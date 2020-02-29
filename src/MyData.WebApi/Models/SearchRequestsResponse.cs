namespace MyData.WebApi.Models
{
    public class SearchRequestsResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public bool HasNextPage { get; set; }
        public string Attachment { get; set; }
    }
}