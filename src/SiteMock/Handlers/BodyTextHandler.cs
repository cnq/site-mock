using CsQuery;
using Microsoft.Owin;
using SiteMock.Middleware;

namespace SiteMock.Handlers
{
    public class BodyTextHandler : IPageContentHandler
    {
        public void Handle(IOwinContext context)
        {
            var document = (CQ)context.Environment["document"];
            CQ div = "<div/>";
            //div.Append(Lorem.Paragraph(30, 50, 10));
            document["body"].Append(div);
        }
    }
}
