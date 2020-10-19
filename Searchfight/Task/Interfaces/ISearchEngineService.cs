using System.Collections.Generic;
using Task.Models;

namespace Task.Interfaces
{
    public interface ISearchEngineService
    {
        System.Threading.Tasks.Task<List<SearchResult>> ProcessSearchQuery(List<string> Userinput);
    }
}
