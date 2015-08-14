using System.Text.RegularExpressions;
using Microsoft.Owin;

namespace SiteMock.Middleware
{

    public class HeadersMiddleware : ServerMiddleware
    {
        public HeadersMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request, "header_.*_.*/");

            ProcessRequest = context =>
            {
                var regex = new Regex("header_(?<header>.*?)_(?<value>.*?)/",RegexOptions.IgnoreCase);
                var matches = regex.Matches(context.Request.Path.Value);
                foreach (Match match in matches)
                {
                    if(match.Groups.Count < 2) continue;
                    context.Response.Headers[match.Groups["header"].Value] = match.Groups["value"].Value;
                }
            };
        }
    }
}
