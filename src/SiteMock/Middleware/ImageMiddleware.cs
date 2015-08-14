using System.Drawing;
using Microsoft.Owin;

namespace SiteMock.Middleware
{
    public class ImageMiddleware : ServerMiddleware
    {
        public ImageMiddleware(OwinMiddleware next)
            : base(next)
        {
            HandleWhen = context => RequestPatterIs(context.Request, @".*\.png$");

            ProcessRequest = context =>
            {
                var img = new Bitmap(300, 300);
                using (var g = Graphics.FromImage(img))
                {
                    g.FillRectangle(new SolidBrush(Color.Aqua), 0, 0, 300, 300);
                }
                var imgArray = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));

                context.Response.ContentType = "image/png";
                context.Response.WriteAsync(imgArray);
            };
        }
    }
}
