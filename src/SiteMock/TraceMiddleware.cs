using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace SiteMock
{
    public class TraceMiddleware : OwinMiddleware
    {

        public TraceMiddleware(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            Trace.TraceInformation("");
            Trace.TraceInformation("");
            Trace.TraceInformation("Request: {0}  {1}", context.Request.Environment["owin.RequestMethod"], context.Request.Path);
            if (!string.IsNullOrEmpty(context.Request.Environment["owin.RequestQueryString"].ToString()))
                Trace.TraceInformation("\tquery string : {0}", context.Request.Environment["owin.RequestQueryString"]);

            Trace.TraceInformation("");
            Trace.TraceInformation("From : {0} (from port:{1})", context.Request.Environment["server.RemoteIpAddress"], context.Request.Environment["server.RemotePort"]);
            Trace.TraceInformation("");
            Trace.TraceInformation("Request Headers:");
            foreach (var header in context.Request.Headers)
            {
                Trace.TraceInformation("\t{0} : {1}", header.Key, header.Value[0]);
            }

            Trace.TraceInformation("");
            Trace.TraceInformation("Additional Info:");
            Trace.TraceInformation("\tscheme : {0}", context.Request.Environment["owin.RequestScheme"]);
            Trace.TraceInformation("\tprotocol : {0}", context.Request.Environment["owin.RequestProtocol"]);
            Trace.TraceInformation("\tlocal request : {0}", context.Request.Environment["server.IsLocal"]);

            using (var reader = new StreamReader(context.Request.Body))
            {
                var body = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(body))
                {
                    Trace.TraceInformation("");
                    Trace.TraceInformation("\tBody :");
                    Trace.TraceInformation("\t{0}", body);
                }
            }

            Trace.TraceInformation("");
            Trace.TraceInformation("");

            await Next.Invoke(context);

            Trace.TraceInformation("");
            Trace.TraceInformation("");
            Trace.TraceInformation("Response Code: {0}", context.Response.StatusCode);

            Trace.TraceInformation("");
            Trace.TraceInformation("Response Headers:");
            foreach (var header in context.Response.Headers)
            {
                Trace.TraceInformation("\t{0} : {1}", header.Key, header.Value[0]);
            }

            Trace.TraceInformation("");
            Trace.TraceInformation("");
            Trace.TraceInformation("-------------------------------------------------------------------------------------------------");
        }

    }
}
