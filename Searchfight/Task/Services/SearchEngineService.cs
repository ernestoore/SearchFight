using Domain.AgentContrats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Task.Interfaces;
using Task.Models;

namespace Task.Services
{
    public class SearchEngineService : ISearchEngineService
    {
        private readonly IServiceSearchAgent _serviceSearchAgent;

        public SearchEngineService(IServiceSearchAgent serviceSearchAgent)
        {
            _serviceSearchAgent = serviceSearchAgent;
        }

        public async  System.Threading.Tasks.Task<List<SearchResult>> ProcessSearchQuery(List<string> Userinput)
        {
            Console.WriteLine("Starting comparison between input words");
            List<SearchResult> lstsearchResults;
            string fixedItem;
            string SearchNumberGoogle;
            string SearchNumberBing;
            int count = 0;
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            lstsearchResults = new List<SearchResult>();

            foreach (var item in Userinput)
            {
                fixedItem = regexItem.IsMatch(item) ? item: item.Trim('"');
                try
                {
                    SearchNumberGoogle = await _serviceSearchAgent.GetGoogleServiceClient(fixedItem);
                    SearchNumberBing = await _serviceSearchAgent.GetBingServiceClient(fixedItem);
                    count++;

                    if (SearchNumberGoogle == null || SearchNumberBing == null)
                    {
                        lstsearchResults = null;
                    }
                    else
                    {
                        SearchResult searchResult = new SearchResult
                        {
                            count = count,
                            SearchedWord = fixedItem,
                            GoogleSearchResult = SearchNumberGoogle,
                            BingSearchResult = SearchNumberBing
                        };
                        lstsearchResults.Add(searchResult);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during execution of SearchEngines:" + ex.Message);
                }
            }

            return lstsearchResults;
        }
    }
}
