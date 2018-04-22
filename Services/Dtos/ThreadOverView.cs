﻿using System;
using System.Collections.Generic;
using EnsureThat;

namespace Services.Dtos
{
    public sealed class ThreadOverView 
    {
        public ThreadOverView(Guid threadId, string subject, PostOverView firstPost, IReadOnlyList<PostOverView> finalPosts, int postCount, int imageCount)
        {
            this.ThreadId = EnsureArg.IsNotEmpty(threadId, nameof(threadId));
            Subject = EnsureArg.IsNotNull(subject, nameof(subject));
            OP = EnsureArg.IsNotNull(firstPost, nameof(firstPost));
            FinalPosts = EnsureArg.IsNotNull(finalPosts, nameof(finalPosts));
            PostCount = EnsureArg.IsGte(postCount, 0, nameof(PostCount));
            ImageCount = EnsureArg.IsGte(imageCount, 0, nameof(ImageCount));
        }

        public Guid ThreadId { get; }

        public string Subject { get; }

        public PostOverView OP { get; }

        public IReadOnlyList<PostOverView> FinalPosts { get; }

        public int PostCount { get; }

        public int ImageCount { get; }
    }
}