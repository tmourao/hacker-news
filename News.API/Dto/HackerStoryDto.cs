﻿using System;

namespace News.API.Dto
{
    public class HackerStoryDto
    {
        public string Title { get; set; }

        public string Uri { get; set; }

        public string PostedBy { get; set; }

        public DateTimeOffset Time { get; set; }

        public int Score { get; set; }

        public int CommentCount { get; set; }

        
    }
}
