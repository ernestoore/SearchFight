using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ResponseBingSearch
    {
        public WebPages webPages { get; set; }
    }
    public class WebPages
    {
        public string webSearchUrl { get; set; }
        public Int64 totalEstimatedMatches { get; set; }
    }
}
