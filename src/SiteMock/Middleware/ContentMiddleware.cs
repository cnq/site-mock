using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Autofac;
using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class ContentMiddleware<THandler> : ServerMiddleware
        where THandler : IContentHandler
    {
        protected Action<IOwinContext> Initialize = context => { };
        protected Action<IOwinContext> Finalize = context => { };
        protected ContentMiddleware(OwinMiddleware next) : base(next)
        {
            ProcessRequest = context =>
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
                Trace.TraceInformation("-------------------------------------------------------------------------------------------------");

                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";



                Initialize.Invoke(context);
                var lifetimeScope = (ILifetimeScope)context.Environment["autofac:OwinLifetimeScope"];
                foreach (var handler in lifetimeScope.Resolve<IEnumerable<THandler>>())
                    handler.Handle(context);

                Finalize.Invoke(context);
            };
        }
    }
}
