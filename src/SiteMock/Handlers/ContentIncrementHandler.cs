using System.Text.RegularExpressions;
using CsQuery;
using Microsoft.Owin;
using SiteMock.Middleware;

namespace SiteMock.Handlers
{
    public class ContentIncrementHandler : IPageContentHandler
    {
        private static int CurrentIncrement = 1;
        public void Handle(IOwinContext context)
        {
            var regex = new Regex(@"increment/.*\.html?$|.*\.php$", RegexOptions.IgnoreCase);
            var match = regex.Match(context.Request.Path.Value);

            if (!match.Success) return;

            var document = (CQ)context.Environment["document"];
                CQ div = "<div/>";
                div.Append(string.Format("Request # {0}", CurrentIncrement ++));
                document["body"].Append(div);
        }
    }
}