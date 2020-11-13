using News.Core.Web.Models.Hacker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Services.Hacker
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerStory>> GetBestStories(int size);
    }
}
