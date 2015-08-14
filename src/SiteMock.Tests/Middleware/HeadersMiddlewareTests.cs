using System;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SiteMock.Middleware;

namespace SiteMock.Tests.Middleware
{
    [TestClass]
    public class HeadersMiddlewareTests
    {
        [TestMethod]
        public void ShouldBeInvokedForMatchingPaths()
        {
            var handlerInvocationTest = new MiddlewareInvocationTest<HeadersMiddleware>();

            Action<Mock<IOwinResponse>, Mock<IOwinContext>, string> verification =
                (response, context, path) => response.VerifyGet(r => r.Headers, Times.AtLeastOnce(), string.Format("The middleware {0} SHOULD have been invoked for path: {1}", typeof(HeadersMiddleware).Name, path));

            //Handler should be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/header_X-Frame-Options_SAMEORIGIN/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/Header_X-Frame-Options_SAMEORIGIN/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somedirectory/header_X-Frame-Options_SAMEORIGIN/oldschool.htm", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/header_X-Frame-Options_SAMEORIGIN/header_Server_theserver/another.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/header_X-Request-Guid_d5845750-c5d9-46fc-84c8-2320f6149f3d/page.php", verification);

            verification =
                (response, context, path) => response.VerifyGet(r => r.Headers, Times.Exactly(0), string.Format("The middleware {0} should NOT have been invoked for path: {1}", typeof(HeadersMiddleware).Name, path));

            //Handler should NOT be invoked for the following request paths
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/X-Request-Guid_d5845750-c5d9-46fc-84c8-2320f6149f3d/page.html", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/somepath/file.xml", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/style/site.css", verification);
            handlerInvocationTest.TestInvokeWhenRequestPathIs("/js/thenextbigthang.js", verification);
        }

    }
}
