using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class DelayMiddleware : ServerMiddleware
    {
        public DelayMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request, "delay_.*/");

            ProcessRequest = context =>
            {
                var regex = new Regex("delay_(?<delayamount>.*?)/", RegexOptions.IgnoreCase);
                var match = regex.Match(context.Request.Path.Value);

                if (match.Groups.Count < 1) return;
                var delayAmount = 0;
                if (int.TryParse(match.Groups["delayamount"].Value, out delayAmount))
                    Thread.Sleep(delayAmount);
            };
        }
    }
}
