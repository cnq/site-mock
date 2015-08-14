using CsQuery;
using Microsoft.Owin;
using SiteMock.Middleware;

namespace SiteMock.Handlers
{
    public class PageTitleHandler : IPageContentHandler
    {
        public void Handle(IOwinContext context)
        {
            var document = (CQ)context.Environment["document"];
            var head = document["head"];
            CQ title = "<title/>";
            title.Append("The title");
            head.Append(title);
        }
    }
}
