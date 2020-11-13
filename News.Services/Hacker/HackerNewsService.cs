using News.Core.Web.Models.Hacker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace News.Services.Hacker
{
    public class HackerNewsService : IHackerNewsService
    {
        #region Private Fields

        private readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public HackerNewsService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<HackerStory>> GetBestStories(int size)
        {
            var data = await _httpClient.GetAsync("/v0/beststories.json");

            // possible exception needs to be handled
            data.EnsureSuccessStatusCode();

            var content = await data.Content.ReadAsStringAsync();
            var bestStoryIds = JsonConvert.DeserializeObject<IEnumerable<int>>(content);

            var detailTasks = bestStoryIds.Select(id => _httpClient.GetAsync($"/v0/item/{id}.json"));
            var detailHttpResponses = await Task.WhenAll(detailTasks);
            var detailContentTasks = detailHttpResponses.Select(httpResponse =>
            {
                httpResponse.EnsureSuccessStatusCode();

                return httpResponse.Content.ReadAsStringAsync();
            });
            var detailContent = await Task.WhenAll(detailContentTasks);

            var bestStories = detailContent.Select(c => JsonConvert.DeserializeObject<HackerStory>(c))
                .OrderByDescending(x => x.Score)
                .Take(size);

            return bestStories;
        }

        #endregion
    }
}
