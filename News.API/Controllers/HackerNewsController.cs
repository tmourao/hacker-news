using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.API.Dto;
using News.Core.Extensions;
using News.Core.Web.Models.Hacker;
using News.Services.Hacker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace News.API.Controllers
{
    [Route("api/v1/hackernews")]
    public class HackerNewsController : ControllerBase
    {
        #region Private Fields

        private readonly IHackerNewsService _hackerNewsService;

        #endregion

        #region Ctor

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService ?? throw new ArgumentNullException(nameof(hackerNewsService));
        }

        #endregion

        #region Gets

        /// <summary>
        /// Get all hacker news best stories
        /// </summary>
        /// <param name="size">Number of stories</param>
        /// <returns></returns>
        [HttpGet("beststories")]
        [ProducesResponseType(typeof(IEnumerable<HackerStoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetBestStories([FromQuery] int size = 20)
        {
            var stories = await _hackerNewsService.GetBestStories(size);

            if (stories.IsNullOrEmpty())
                return NoContent();

            var storiesDto = stories.Select(x => new HackerStoryDto
            {
                Title = x.Title,
                Uri = x.Url,
                PostedBy = x.By,
                Time = x.GetTime(),
                Score = x.Score,
                CommentCount = x.Descendants
            });

            return Ok(storiesDto);
        }

        #endregion
    }
}
