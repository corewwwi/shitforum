﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Dtos;
using Services.Interfaces;

namespace ShitForum.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly IThreadService threadService;

        public CatalogModel(IThreadService threadService)
        {
            this.threadService = threadService;
        }

        public async Task<IActionResult> OnGet(string boardKey, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotEmpty(boardKey, nameof(boardKey));
            var t = await this.threadService.GetOrderedCatalogThreads(boardKey, cancellationToken);
            return t.Match(threads =>
            {
                this.Threads = threads.Threads;
                this.Board = threads.Board;
                return Page().ToIAR();
            },
            () => this.NotFound().ToIAR());
        }

        public IEnumerable<CatalogThreadOverView> Threads { get; private set; }
        
        public Board Board { get; private set; }
    }
}
