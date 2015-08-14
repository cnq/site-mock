using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class CssContentMiddleware : ContentMiddleware<ICssHandler>
    {
        public CssContentMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request,@".*\.css$");

            Initialize = context =>
            {
                context.Response.ContentType = "text/css";
            };
            Finalize = context =>
            {
                context.Response.Write(@"body{}");
            };
        }
    }
}