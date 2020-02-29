using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;
using MyData.WebApi.Models;

namespace MyData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestStore _requestStore;

        private readonly ILogger<RequestsController> _logger;

        public RequestsController(IRequestStore requestStore, ILogger<RequestsController> logger)
        {
            _requestStore = requestStore;
            _logger = logger;
        }

        public async Task<SearchRequestsResponse> Search([FromQuery] SearchRequestsArgs searchArgs)
        {
            _logger.LogInformation("Performing search with args: {0}",searchArgs.ToString());

            var requests = await _requestStore.SearchAsync(searchArgs.FromInclusive, searchArgs.ToInclusive,
                searchArgs.PageNumber, searchArgs.PageSize);

            var response = new SearchRequestsResponse
            {
                PageNumber = requests.PageNumber,
                PageSize = requests.PageSize,
                HasNextPage = requests.HasNextPage,
                Count = requests.Count,
                Attachment = string.Empty
            };

            if (requests.Count == 0) return response;
            
            var json = JsonSerializer.SerializeToUtf8Bytes(requests.ToArray());
            response.Attachment = await ToGzip(json);
            return response;
        }

        private async Task<string> ToGzip(byte[] bytes)
        {
            await using var outputStream = new MemoryStream();
            await using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                gZipStream.Write(bytes, 0, bytes.Length);

            var outputBytes = outputStream.ToArray();
            return Convert.ToBase64String(outputBytes);
        }
    }
}