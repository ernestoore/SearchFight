using Domain.AgentContrats;
using Moq;
using Searchfight;
using System;
using System.Collections.Generic;
using Task.Models;
using Task.Services;
using Xunit;

namespace SearchFight.Moq
{
    public class SearchEngineServiceTest
    {
        private readonly SearchEngineService _sut;
        private readonly Mock<IServiceSearchAgent> _serviceSearchAgent = new Mock<IServiceSearchAgent>();

        public SearchEngineServiceTest()
        {
            _sut = new SearchEngineService(_serviceSearchAgent.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task ProcessSearchQueryAsync_Should_Return_ListSearchResults()
        {
            //Arrange
            var lstSearchWords = new List<string>() { "javascript" };
            string userInput = "javascript";
            var lstResults = new List<SearchResult>
            {
                new SearchResult{count = 1, SearchedWord = userInput, GoogleSearchResult= "1000", BingSearchResult = "1500"}
            };


            _serviceSearchAgent.Setup(x => x.GetGoogleServiceClient(userInput))
                .ReturnsAsync("1000");

            _serviceSearchAgent.Setup(x => x.GetBingServiceClient(userInput))
                .ReturnsAsync("1500");
            //Act
            var lstSearchResults = await _sut.ProcessSearchQuery(lstSearchWords);

            //Asserts
            Assert.Equal(lstResults[0].BingSearchResult, lstSearchResults[0].BingSearchResult);
            Assert.Equal(lstResults[0].GoogleSearchResult, lstSearchResults[0].GoogleSearchResult);
        }


        [Fact]
        public async System.Threading.Tasks.Task ProcessSearchQueryAsync_Should_ReturnNUll_WhenSearchAgentError()
        {
            //Arrange
            var lstSearchWords = new List<string>() { "javascript" };

            _serviceSearchAgent.Setup(x => x.GetGoogleServiceClient(It.IsAny<String>()))
                .ReturnsAsync(() => null);

            _serviceSearchAgent.Setup(x => x.GetBingServiceClient(It.IsAny<String>()))
                .ReturnsAsync(() => null);
            //Act
            var lstSearchResults = await _sut.ProcessSearchQuery(lstSearchWords);

            //Asserts
            Assert.Null(lstSearchResults);
        }
    }
}
