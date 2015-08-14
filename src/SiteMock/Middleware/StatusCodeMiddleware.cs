using System.Text.RegularExpressions;
using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class StatusCodeMiddleware : ServerMiddleware
    {
        public StatusCodeMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request, "statuscode_.*/");

            ProcessRequest = context =>
            {
                var regex = new Regex("statuscode_(?<statuscode>.*?)/", RegexOptions.IgnoreCase);
                var match = regex.Match(context.Request.Path.Value);
                if (match.Groups.Count < 1) return;
                var statusCode = 0;
                if (int.TryParse(match.Groups["statuscode"].Value, out statusCode))
                    context.Response.StatusCode = statusCode;
            };
        }
    }
}
