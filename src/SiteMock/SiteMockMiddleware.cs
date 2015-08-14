using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace SiteMock
{
    public class ServerMiddleware : OwinMiddleware
    {
        protected Func<IOwinContext, bool> HandleWhen = context => false;
        protected Action<IOwinContext> ProcessRequest = context => { };
        protected bool StopPropagating { get; set; }

        protected ServerMiddleware(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            if (HandleWhen(context))
            {
                ProcessRequest(context);
            }
            if (!StopPropagating)
                await Next.Invoke(context);
        }


        protected bool RequestPatterIs(IOwinRequest request, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(request.Path.Value);
        }
    }
}
