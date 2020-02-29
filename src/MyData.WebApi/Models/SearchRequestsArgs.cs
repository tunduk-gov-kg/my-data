using System;
using System.ComponentModel.DataAnnotations;

namespace MyData.WebApi.Models
{
    public class SearchRequestsArgs
    {
        [Required] public DateTime FromInclusive { get; set; }

        [Required] public DateTime ToInclusive { get; set; }

        [Range(1, int.MaxValue)] public int PageNumber { get; set; }

        [Range(1, 10_000)] public int PageSize { get; set; }

        public override string ToString()
        {
            return $"FromInclusive - {FromInclusive:O}, ToInclusive - {ToInclusive:O}, PageNumber - {PageNumber}, PageSize - {PageSize}";
        }
    }
}