using System.Text.RegularExpressions;
using CsQuery;
using Microsoft.Owin;
using SiteMock.Middleware;

namespace SiteMock.Handlers
{
    public class PageDiagnosticsHandler : IPageContentHandler
    {
        public void Handle(IOwinContext context)
        {
            var regex = new Regex(@"diagnostics", RegexOptions.IgnoreCase);
            var match = regex.Match(context.Request.Path.Value);

            if (!match.Success) return;

            var document = (CQ)context.Environment["document"];

            document["body"].Append("<p/>");
            document["body"].Append("<p/>");

            CQ div = "<div/>";
            div.Append(string.Format("Request Method : {0}", context.Request.Environment["owin.RequestMethod"]));
            document["body"].Append(div);

            div = "<div/>";
            div.Append(string.Format("Request Scheme : {0}", context.Request.Environment["owin.RequestScheme"]));
            document["body"].Append(div);

            div = "<div/>";
            div.Append(string.Format("Request Protocol : {0}", context.Request.Environment["owin.RequestProtocol"]));
            document["body"].Append(div);

            div = "<div/>";
            div.Append(string.Format("Remote IP Address : {0}", context.Request.Environment["server.RemoteIpAddress"]));
            document["body"].Append(div);

            div = "<div/>";
            div.Append(string.Format("Remote Port : {0}", context.Request.Environment["server.RemotePort"]));
            document["body"].Append(div);

            document["body"].Append("<p/>");

            foreach (var header in context.Request.Headers)
            {
                div = "<div/>";
                div.Append(string.Format("{0} : {1}", header.Key, header.Value[0]));
                document["body"].Append(div);
            }


            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
        }
    }
}