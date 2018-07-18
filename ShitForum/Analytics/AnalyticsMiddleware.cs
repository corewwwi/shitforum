using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System;
using Cookies;
using ExtremeIpLookup;
using Hashers;
using Services.Interfaces;

namespace ShitForum.Analytics
{
    public class AnalyticsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAnalyticsService svc;
        private readonly IExtremeIpLookup ipLookup;
        private readonly ICookieStorage cookies;

        public AnalyticsMiddleware(RequestDelegate next, IAnalyticsService svc, IExtremeIpLookup ipLookup, ICookieStorage cookies)
        {
            this.next = next;
            this.svc = svc;
            this.ipLookup = ipLookup;
            this.cookies = cookies;
        }

        private static Guid GetThumbPrint(ICookieStorage cs, HttpRequest req, HttpResponse res)
        {
            var thumb = cs.ReadThumbPrint(req);
            return thumb.Match(some => some, () =>
            {
                var newThumb = Guid.NewGuid();
                cs.SetThumbPrint(res, newThumb);
                return newThumb;
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress;
            var deats = await ipLookup.GetIpDetailsAsync(ip);
            
            await deats.Match(o =>
            {
                var thumb = GetThumbPrint(cookies, context.Request, context.Response);
                return svc.Add(new Domain.AnalyticsReport(Guid.NewGuid(), DateTime.UtcNow, o.City, Sha256Hasher.Hash(thumb.ToString())), CancellationToken.None);
            }, _ => Task.CompletedTask);
            
            await this.next(context);
        }
    }
}
