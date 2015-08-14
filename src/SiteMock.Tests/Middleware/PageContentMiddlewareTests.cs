using System;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SiteMock.Middleware;

namespace SiteMock.Tests.Middleware
{
    [TestClass]
    public class PageContentMiddlewareTests
    {
        [TestMethod]
        public void ShouldBeInvokedForMatchingPaths()
        {
            var handlerInvocationTest = new MiddlewareInvocationTest<PageContentMiddleware>();

            Action<Mock<IOwinResponse>, Mock<IOwinContext>, string> verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(1), string.Format("The handler {0} SHOULD have been invoked for path: {1}", typeof(PageContentMiddleware).Name, path));

            //Handler should be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somedirectory/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somedirectory/oldschool.htm", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/someotherdirectory/another.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/some/long/path/with/a/page/at/the/end.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/directory/page.php", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("", verification);

            verification =
                (response, context, path) => response.Verify(r => r.Write(It.IsAny<string>()), Times.Exactly(0), string.Format("The handler {0} should NOT have been invoked for path: {1}", typeof(PageContentMiddleware).Name, path));

            //Handler should NOT be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/path-without-extension", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somepath/file.xml", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/style/site.css", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/js/thenextbigthang.js", verification);
        }

    }
}
