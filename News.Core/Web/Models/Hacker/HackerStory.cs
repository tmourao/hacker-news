using Newtonsoft.Json;
using System;

namespace News.Core.Web.Models.Hacker
{
    public class HackerStory
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string By { get; set; }

        public int Score { get; set; }

        public int Descendants { get; set; }

        /// <summary>
        /// Unix Time
        /// </summary>
        public long Time { get; set; }

        public DateTimeOffset GetTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(Time);
        }
    }
}
