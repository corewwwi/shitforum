﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.IpHash;
using OneOf;
using Optional;
using Services.Dtos;
using Services.Results;

namespace Services.Interfaces
{
    public interface IPostService
    {
        //todo primitive obsession
        Task<OneOf<Success, Banned, ImageCountExceeded, PostCountExceeded>> Add(Guid postId, Guid threadId, TripCodedName name, string comment, bool isSage, IIpHash ipAddress, Option<File> file, CancellationToken cancellationToken);

        Task<Option<PostContextView>> GetById(Guid id, CancellationToken cancellationToken);

        Task<OneOf<Success, Banned>> AddThread(Guid postId, Guid threadId, Guid boardId, string subject, TripCodedName name, string comment, bool isSage, IIpHash ipAddress, Option<File> file, CancellationToken cancellationToken);

        Task<bool> DeletePost(Guid id, CancellationToken cancellationToken);
    }
}
