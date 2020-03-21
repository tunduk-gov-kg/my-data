using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.WebApi.BackgroundService;
using Nito.AsyncEx.Synchronous;

namespace MyData.WebApi
{
    public class LogsCollectorService
    {
        private readonly IJournalService _journalService;

        private readonly LogsCollectorOptions _options;

        private readonly IXRoadDbReader _xRoadDbReader;

        private readonly IXRoadLogsProcessor _logsProcessor;

        private readonly IXRoadRequestStore _requestStore;

        private readonly ILogger<LogsCollectorService> _logger;

        public LogsCollectorService(IJournalService journalService, IOptions<LogsCollectorOptions> options,
            IXRoadLogsProcessor logsProcessor, IXRoadDbReader xRoadDbReader, IXRoadRequestStore requestStore,
            ILogger<LogsCollectorService> logger)
        {
            _journalService = journalService;
            _logsProcessor = logsProcessor;
            _xRoadDbReader = xRoadDbReader;
            _requestStore = requestStore;
            _logger = logger;
            _options = options.Value;
        }


        public async Task Collect(XRoadLogsDb targetDb)
        {
            var lastSucceededCollectorOperation = await _journalService.GetLastSucceededOperationAsync(targetDb.Host);

            var isFirstConnection = lastSucceededCollectorOperation == null;

            var fromIdInclusive = isFirstConnection ? 0L : lastSucceededCollectorOperation.LastRecordId + 1;
            
            for (var iteration = 0; iteration < _options.CollectorIterationsCount; iteration++)
            {
                var journal = new JournalRecordBuilder()
                    .SetXRoadDb(targetDb)
                    .SetFromIdInclusive(fromIdInclusive)
                    .SetLimit(_options.Limit);

                try
                {
                    var anyRecords = _xRoadDbReader.AnyRecords(targetDb, fromIdInclusive);

                    if (!anyRecords)
                    {
                        //just ignore
                        break;
                    }

                    var collectResult = Collect(targetDb, fromIdInclusive, _options.Limit);

                    fromIdInclusive = collectResult.LastRecordId + 1;
                    journal.SetLastRecordId(collectResult.LastRecordId);
                    journal.SetParsedCount(collectResult.ParsedCount);
                    await _journalService.AddAsync(journal.Build());
                }
                catch (Exception exception)
                {
                    journal.SetException(exception);
                    _logger.LogError(exception, "Error occurred LogsCollectorService execution");
                    await _journalService.AddAsync(journal.Build());
                    break;
                }
            }
        }

        private CollectResult Collect(XRoadLogsDb targetDb, long fromIdInclusive, int limit)
        {
            var collectResult = new CollectResult();

            var logsReadResult = _xRoadDbReader.Read(targetDb, fromIdInclusive, limit);

            if (!logsReadResult.Any())
            {
                return collectResult;
            }

            var xRoadRequests = _logsProcessor.Process(logsReadResult);

            _requestStore.AddRangeAsync(xRoadRequests).WaitAndUnwrapException();

            collectResult.Count = logsReadResult.Count;

            collectResult.ParsedCount = xRoadRequests.Count;

            collectResult.LastRecordId = logsReadResult.LastRecordId;

            return collectResult;
        }

        private class CollectResult
        {
            public long Count { get; set; }

            public int ParsedCount { get; set; }

            public long LastRecordId { get; set; } = -1;
        }
    }
}