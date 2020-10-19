using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Models
{
    public class SearchResult
    {
        public int count { get; set; }
        public String SearchedWord { get; set; }
        public String GoogleSearchResult { get; set; }
        public String BingSearchResult { get; set; }
    }
}
