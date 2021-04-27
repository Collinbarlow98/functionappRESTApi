using System;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using FunctionApp2.Options;
using FunctionApp2.Models;
using Microsoft.Extensions.Options;

namespace FunctionApp2.Searches
{
    public class Search
    {
        private readonly MyOptions _settings;

        public Search(IOptions<MyOptions> options)
        {
            _settings = options.Value;
        }

        public SearchResults<TohHero> SearchHeroes(string searchTerm)
        {
            SearchClient indexClientForQueries = CreateSearchClientForQueries(_settings.indexName);

            return RunQueries(indexClientForQueries, searchTerm);
        }

        private SearchIndexClient CreateSearchIndexClient()
        {
            SearchIndexClient indexClient = new SearchIndexClient(new Uri(_settings.searchUri), new AzureKeyCredential(_settings.searchKey));

            return indexClient;
        }

        private SearchClient CreateSearchClientForQueries(string indexName)
        {
            SearchClient searchClient = new SearchClient(new Uri(_settings.searchUri), indexName, new AzureKeyCredential(_settings.searchKey));

            return searchClient;
        }

        private SearchResults<TohHero> RunQueries(SearchClient searchClient, string searchTerm)
        {
            SearchOptions options;
            SearchResults<TohHero> results;

            options = new SearchOptions()
            {
                SearchFields = { "Name" }
            };

            options.Select.Add("Name");
            options.Select.Add("HeroId");
            options.Select.Add("Likes");

            results = searchClient.Search<TohHero>(searchTerm, options);
            return results;
        }
    }
}
