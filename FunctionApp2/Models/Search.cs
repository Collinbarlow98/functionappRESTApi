using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using ToHApi.Models;

namespace FunctionApp2.Models
{
    public class Search
    {
        public static SearchResults<TohHero> Main(string searchTerm)
        {
            string indexName = "azuresql-index";

            SearchClient indexClientForQueries = CreateSearchClientForQueries(indexName);

            return RunQueries(indexClientForQueries, searchTerm);
        }

        private static SearchIndexClient CreateSearchIndexClient()
        {
            SearchIndexClient indexClient = new SearchIndexClient(new Uri("https://tohsearch.search.windows.net"),  new AzureKeyCredential("15757E23F998C26FA3804F0BBFAB8F2C"));

            return indexClient;
        }
        private static SearchClient CreateSearchClientForQueries(string indexName)
        {
            SearchClient searchClient = new SearchClient(new Uri("https://tohsearch.search.windows.net"), indexName, new AzureKeyCredential("15757E23F998C26FA3804F0BBFAB8F2C"));

            return searchClient;
        }

        private static SearchResults<TohHero> RunQueries(SearchClient searchClient, string searchTerm)
        {
            SearchOptions options;
            SearchResults<TohHero> results;

            options = new SearchOptions()
            {
                SearchFields = { "Name" }
            };

            options.Select.Add("Name");
            options.Select.Add("Id");
            options.Select.Add("Likes");

            results = searchClient.Search<TohHero>(searchTerm, options);
            return results;
        }
    }
}
