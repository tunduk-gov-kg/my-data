using System;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyData.Core.Interfaces;
using MyData.Core.Models;

namespace MyData.WebApi.BackgroundService
{
    public class LogsCollector : IInvocable
    {
        private readonly IXRoadLogsDbListProvider _xRoadDbListProvider;

        private readonly LogsCollectorOptions _options;

        private readonly IServiceProvider _serviceProvider;

        public LogsCollector(IXRoadLogsDbListProvider xRoadDbListProvider,
            IOptions<LogsCollectorOptions> options, IServiceProvider serviceProvider)
        {
            _xRoadDbListProvider = xRoadDbListProvider;
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke()
        {
            foreach (var logsDb in _xRoadDbListProvider.List.AsParallel())
            {
                await Collect(logsDb);
            }
        }

        private async Task Collect(XRoadLogsDb xRoadLogsDb)
        {
            using var scope = _serviceProvider.CreateScope();

            var journalService = scope.ServiceProvider.GetRequiredService<IJournalService>();
            var xRoadDbReader = scope.ServiceProvider.GetRequiredService<IXRoadDbReader>();
            var xRoadLogsProcessor = scope.ServiceProvider.GetRequiredService<IXRoadLogsProcessor>();
            var requestStore = scope.ServiceProvider.GetRequiredService<IXRoadRequestStore>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<LogsCollector>>();

            var journalRecord = await journalService.LastOrDefaultAsync(xRoadLogsDb.Host);
            
            long fromIdInclusive = NextFromIdInclusive(journalRecord);

            for (int i = 0; i < _options.CollectorIterationsCount; i++)
            {
                long toIdInclusive = fromIdInclusive + _options.LogsCountPerIteration;

                var newJournalRecord = new JournalRecord
                {
                    DbHost = xRoadLogsDb.Host, DbPort = xRoadLogsDb.Port, FromIdInclusive = fromIdInclusive
                };

                try
                {
                    var readResult = xRoadDbReader.Read(xRoadLogsDb, fromIdInclusive, toIdInclusive);
                    var requests = xRoadLogsProcessor.Process(readResult.XRoadLogs);
                    await requestStore.AddRangeAsync(requests);

                    newJournalRecord.Succeeded = true;
                    newJournalRecord.ActualCount = requests.Count;

                    if (readResult.IsLast)
                    {
                        if (readResult.XRoadLogs.Any())
                        {
                            newJournalRecord.ToIdInclusive = readResult.LastRecordId + 1;                            
                        }
                        else
                        {
                            newJournalRecord.ToIdInclusive = fromIdInclusive;
                        }

                        break;
                    }
                    
                    fromIdInclusive = toIdInclusive + 1;
                }
                catch (Exception exception)
                {
                    logger.LogError(exception,
                        "Error occurred during logs collecting for: {0}, from_id_inclusive: {1}, to_id_inclusive: {1}",
                        xRoadLogsDb.Host, fromIdInclusive, toIdInclusive);

                    newJournalRecord.Succeeded = false;
                    newJournalRecord.ActualCount = 0;
                    newJournalRecord.ErrorCode = exception.GetType().FullName;
                    newJournalRecord.ErrorDescription = exception.Message;

                    break;
                }
                finally
                {
                    await journalService.AddAsync(newJournalRecord);
                }
            }
        }

        private static long NextFromIdInclusive(JournalRecord journalRecord)
        {
            long fromIdInclusive = 0;

            if (journalRecord == null) return fromIdInclusive;

            if (journalRecord.Succeeded)
            {
                fromIdInclusive = journalRecord.ToIdInclusive + 1;
            }
            else
            {
                fromIdInclusive = journalRecord.FromIdInclusive;
            }

            return fromIdInclusive;
        }
    }
}