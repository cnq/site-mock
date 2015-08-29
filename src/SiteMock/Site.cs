using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Microsoft.Owin.Hosting;
using Owin;
using SiteMock.Middleware;

namespace SiteMock
{
    public class Site : IDisposable
    {
        private readonly IDisposable _server;

        public Site()
            : this("http://localhost:8888/")
        {
        }

        public Site(string url)
        {
            _server = WebApp.Start(url, (app) =>
            {
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(type => typeof(IContentHandler).IsAssignableFrom(type))
                   .AsImplementedInterfaces();

                app.UseAutofacMiddleware(builder.Build());
                app.Use((context, next) =>
                {
                    context.Environment["Logger"] = new Logger();

                    return next();
                });
                //app.Use<TraceMiddleware>();
                app.Use<ImageMiddleware>();
                app.Use<PageContentMiddleware>();
                app.Use<CssContentMiddleware>();
                app.Use<JavascriptContentMiddleware>();
                app.Use<DelayMiddleware>();
                app.Use<HeadersMiddleware>();
                app.Use<StatusCodeMiddleware>();
            });
            Trace.WriteLine(string.Format("Site Started at {0}", url));
        }
        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
