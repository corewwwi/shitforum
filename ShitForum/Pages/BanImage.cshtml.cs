﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Dtos;
using ShitForum.Attributes;
using ShitForum.Hasher;

namespace ShitForum.Pages
{
    [CookieAuth]
    public class BanImageModel : PageModel
    {
        private readonly IPostService postService;
        private readonly IFileService fileService;

        public BanImageModel(IFileService fileService, IPostService postService)
        {
            this.fileService = fileService;
            this.postService = postService;
        }

        [BindProperty] public string Reason { get; set; }
        public PostContextView Post { get; private set; }

        public async Task<IActionResult> OnGet(Guid id, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotEmpty(id, nameof(id));
            var f = await this.fileService.GetPostFile(id, cancellationToken);
            return await f.Match(async some =>
            {
                var p = await this.postService.GetById(some.Id, cancellationToken);
                return p.Match(post =>
                {
                    this.Post = post;
                    return Page();
                }, new NotFoundResult().ToIAR);
            }, () => new NotFoundResult().ToIART());
        }

        public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotEmpty(id, nameof(id));
            var f = await this.fileService.GetPostFile(id, cancellationToken);

            return await f.Match(async some =>
            {
                var hash = ImageHasher.Hash(some.Data);
                await fileService.BanImage(hash, Reason, cancellationToken);
                return base.RedirectToPage("Index").ToIAR();
            }, () => new NotFoundResult().ToIART());
        }
    }
}
