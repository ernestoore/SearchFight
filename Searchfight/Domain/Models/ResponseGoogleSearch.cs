using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ResponseGoogleSearch
    {
        public Queries queries { get; set; }
    }

    public class Queries
    {
        public List<Request> request { get; set; }
    }

    public class Request
    {
        public string totalResults { get; set; }
    }
}
