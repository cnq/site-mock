using System;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SiteMock.Middleware;

namespace SiteMock.Tests.Middleware
{
    [TestClass]
    public class CssContentMiddlewareTests
    {
        [TestMethod]
        public void ShouldBeInvokedForMatchingPaths()
        {
            var handlerInvocationTest = new MiddlewareInvocationTest<CssContentMiddleware>();


            Action<Mock<IOwinResponse>, Mock<IOwinContext>, string> verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(1), string.Format("The handler {0} SHOULD have been invoked for path: {1}", typeof(CssContentMiddleware).Name, path));

            //Handler should be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/style/wizbang.css", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/path/of/any/length/site.css", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/base-path-file.css", verification);

            verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(0), string.Format("The handler {0} should NOT have been invoked for path: {1}", typeof(CssContentMiddleware).Name, path));

            //Handler should NOT be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somedirectory/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/css/not-a-css-file.csss", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/a-tricky-path.css.txt", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/versioned-css-not-supported-at-this-time.css??any-kind-of-query-string-that-is-used-to-implement-a-versioning-strategy=vNext!!", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("", verification);
        }

    }
}
