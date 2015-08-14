using System.Text.RegularExpressions;
using CsQuery;
using Microsoft.Owin;
using SiteMock.Middleware;

namespace SiteMock.Handlers
{
    public class PageJavascriptHandler : IPageContentHandler
    {
        public void Handle(IOwinContext context)
        {
            var regex = new Regex(@"addjs_(?<count>.*?)/.*\.html?$|.*\.php$", RegexOptions.IgnoreCase);
            var match = regex.Match(context.Request.Path.Value);

            if (!match.Success || match.Groups.Count < 1) return;
            var count = 0;
            if (!int.TryParse(match.Groups["count"].Value, out count)) return;

            var document = (CQ)context.Environment["document"];
            for (var i = 0; i < count; i++)
            {
                CQ div = "<div/>";
                div.Append(string.Format("<script type='text/javascript' src='/js/script{0}.js'></script>", i));
                document["body"].Append(div);
            }
        }
    }
}