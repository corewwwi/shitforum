﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ShitForum
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .UseStartup<Startup>()
                .Build();
    }
}
