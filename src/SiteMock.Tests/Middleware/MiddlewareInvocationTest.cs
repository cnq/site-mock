using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SiteMock.Tests.Middleware
{
    public class MiddlewareInvocationTest<T>
        where T : OwinMiddleware
    {
        public void TestInvokeWhenRequestPathIs(string path, Action<Mock<IOwinResponse>,Mock<IOwinContext>,string> verification)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IContentHandler).Assembly)
                    .Where(type => typeof(IContentHandler).IsAssignableFrom(type))
                    .AsImplementedInterfaces();

            var container = builder.Build();
            var lifetimeScope = container.BeginLifetimeScope();

            var middleware = new Mock<OwinMiddleware>(null);
            var request = new Mock<IOwinRequest>();
            var response = new Mock<IOwinResponse>();
            var context = new Mock<IOwinContext>();

            middleware.Setup(m => m.Invoke(It.IsAny<IOwinContext>())).Returns(Task.FromResult(0));
            request.SetupGet(r => r.Path).Returns(new PathString(path));
            response.SetupGet(r => r.Headers).Returns(new HeaderDictionary(new Dictionary<string, string[]>()));
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Environment).Returns(new Dictionary<string, object>() { { "autofac:OwinLifetimeScope", lifetimeScope } });

            var handler = (T)Activator.CreateInstance(typeof(T), middleware.Object);
            var task = handler.Invoke(context.Object);

            Assert.IsNotNull(task);
            task.Wait();

            verification(response, context, path);
        }
    }
}
