using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace SiteMock
{
    public class ServerMiddleware : OwinMiddleware
    {
        protected Func<IOwinContext, bool> HandleWhen = context => false;
        protected Action<IOwinContext> ProcessRequest = context => { };
        protected bool StopPropagating { get; set; }

        protected ServerMiddleware(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            if (HandleWhen(context))
            {
                var logger = (Logger)context.Environment["Logger"];
                logger.Log("");
                logger.Log("");
                logger.Log("Request: {0}  {1}", context.Request.Environment["owin.RequestMethod"], context.Request.Path);
                if (!string.IsNullOrEmpty(context.Request.Environment["owin.RequestQueryString"].ToString()))
                    logger.Log("\tquery string : {0}", context.Request.Environment["owin.RequestQueryString"]);

                logger.Log("");
                logger.Log("From : {0} (from port:{1})", context.Request.Environment["server.RemoteIpAddress"], context.Request.Environment["server.RemotePort"]);
                logger.Log("");
                logger.Log("Request Headers:");
                foreach (var header in context.Request.Headers)
                {
                    logger.Log("\t{0} : {1}", header.Key, header.Value[0]);
                }

                logger.Log("");
                logger.Log("Additional Info:");
                logger.Log("\tscheme : {0}", context.Request.Environment["owin.RequestScheme"]);
                logger.Log("\tprotocol : {0}", context.Request.Environment["owin.RequestProtocol"]);
                logger.Log("\tlocal request : {0}", context.Request.Environment["server.IsLocal"]);

                using (var reader = new StreamReader(context.Request.Body))
                {
                    var body = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(body))
                    {
                        logger.Log("");
                        logger.Log("\tBody :");
                        logger.Log("\t{0}", body);
                    }
                }

                logger.Log("");
                logger.Log("");


                ProcessRequest(context);


                logger.Log("");
                logger.Log("");
                logger.Log("Response Code: {0}", context.Response.StatusCode);

                logger.Log("");
                logger.Log("Response Headers:");
                foreach (var header in context.Response.Headers)
                {
                    logger.Log("\t{0} : {1}", header.Key, header.Value[0]);
                }

                logger.Log("");
                logger.Log("");
                logger.Log("-------------------------------------------------------------------------------------------------");
                logger.Flush();


            }
            if (!StopPropagating)
                await Next.Invoke(context);
        }


        protected bool RequestPatterIs(IOwinRequest request, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(request.Path.Value);
        }
    }
}
