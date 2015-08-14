using System;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SiteMock.Middleware;

namespace SiteMock.Tests.Middleware
{
    [TestClass]
    public class JavascriptContentMiddlewareTests
    {
        [TestMethod]
        public void ShouldBeInvokedForMatchingPaths()
        {
            var handlerInvocationTest = new MiddlewareInvocationTest<JavascriptContentMiddleware>();

            Action<Mock<IOwinResponse>, Mock<IOwinContext>, string> verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(1), string.Format("The handler {0} SHOULD have been invoked for path: {1}", typeof(JavascriptContentMiddleware).Name, path));
            //Handler should be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/js/thenextbigthang.js", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/path/of/any/length/site.js", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/base-path-file.js", verification);

            verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(0), string.Format("The handler {0} should NOT have been invoked for path: {1}", typeof(JavascriptContentMiddleware).Name, path));

            //Handler should NOT be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somedirectory/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/scripts/not-a-javascript-file.jsx", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/a-tricky-path.js.txt", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/versioned-js-not-supported-at-this-time.js?any-kind-of-query-string-that-is-used-to-implement-a-versioning-strategy=vNext!!", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("", verification);
        }

    }
}
