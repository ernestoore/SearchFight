using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Task.Interfaces;
using Task.Models;

namespace Searchfight
{
    public class App
    {

        private readonly ISearchEngineService _searchService;
        public App(ISearchEngineService searchService)
        {
            _searchService = searchService;
        }
        public async System.Threading.Tasks.Task StartQuerySearch()
        {
            Console.WriteLine("Please write words/terms you would like to compare");
            string userInput;
            List<SearchResult> lstSearchResults;
            userInput = Console.ReadLine();

            var parts = Regex.Matches(userInput, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            try
            {
               

                lstSearchResults = await _searchService.ProcessSearchQuery(parts);

                if (lstSearchResults != null)
                {


                    Console.WriteLine("{0,5} {1,10} {2,15} {3,20}", "#", "Searched word", "Google", "Bing");
                    foreach (var item in lstSearchResults)
                    {
                        Console.WriteLine("{0,5} {1,10} {2,20} {3,25}", item.count, item.SearchedWord, item.GoogleSearchResult, item.BingSearchResult);
                    }


                    SearchResult googleWinner = lstSearchResults.OrderByDescending(item => Int64.Parse(item.GoogleSearchResult)).First();
                    Console.WriteLine("Google winner: " + googleWinner.SearchedWord + " - Total search results: " + Int64.Parse(googleWinner.GoogleSearchResult));
                    SearchResult bingWinner = lstSearchResults.OrderByDescending(item => Int64.Parse(item.BingSearchResult)).First();
                    Console.WriteLine("Bing winner: " + bingWinner.SearchedWord + " -  Total search results: " + Int64.Parse(bingWinner.BingSearchResult));
                    SearchResult totalWinnner = lstSearchResults.OrderByDescending(item => (Int64.Parse(item.GoogleSearchResult) + Int64.Parse(item.BingSearchResult))).First();
                    Console.WriteLine("Total winner: " + totalWinnner.SearchedWord + " -  Total search results: " + (Int64.Parse(totalWinnner.GoogleSearchResult) + Int64.Parse(totalWinnner.BingSearchResult)));
                }
                else
                {
                    Console.WriteLine("Unfortunately couldn't process the request.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

        }
    }
}

