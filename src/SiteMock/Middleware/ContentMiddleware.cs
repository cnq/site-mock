using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Trace.TraceInformation("Request Path: {0}", context.Request.Path);
                Trace.TraceInformation("Request Headers:");
                foreach (var header in context.Request.Headers)
                {
                    Trace.TraceInformation("\t{0} : {1}", header.Key,header.Value[0]); 
                }
                Trace.TraceInformation("");
                Trace.TraceInformation("Environment Values:");
                foreach (var item in context.Request.Environment)
                {
                    Trace.TraceInformation("\t{0} : {1}", item.Key, item.Value);
                }
                Trace.TraceInformation("");
                Trace.TraceInformation("");

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
