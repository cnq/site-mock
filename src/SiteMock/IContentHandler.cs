using Microsoft.Owin;

namespace SiteMock
{
    public interface IContentHandler
    {
        void Handle(IOwinContext context);
    }
}
