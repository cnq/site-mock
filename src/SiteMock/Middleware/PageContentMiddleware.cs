using CsQuery;
using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class PageContentMiddleware : ContentMiddleware<IPageContentHandler>
    {
        public PageContentMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request, @".*\.html?$|.*\.php$|^/$|(^$)");

            Initialize = context =>
            {
                var document = CQ.CreateDocument("");
                document.AttrSet(new { lang = "en" });
                context.Response.ContentType = "text/html";
                context.Environment["document"] = document;
            };
            Finalize = context =>
            {
                var document = (CQ)context.Environment["document"];
                context.Response.StatusCode = 200;
                context.Response.Write(document.Render());
            };
        }
    }
}