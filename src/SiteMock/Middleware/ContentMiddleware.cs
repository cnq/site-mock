using System;
using System.Collections.Generic;
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
                Initialize.Invoke(context);
                var lifetimeScope = (ILifetimeScope)context.Environment["autofac:OwinLifetimeScope"];
                foreach (var handler in lifetimeScope.Resolve<IEnumerable<THandler>>())
                    handler.Handle(context);

                Finalize.Invoke(context);
            };
        }
    }
}
