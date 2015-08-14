using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class JavascriptContentMiddleware : ContentMiddleware<IJavascriptHandler>
    {
        public JavascriptContentMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request,@".*\.js$");

            Initialize = context =>
            {
                context.Response.ContentType = "application/javascript";
            };
            Finalize = context =>
            {
                context.Response.Write(@"//some js");
            };
        }
    }
}