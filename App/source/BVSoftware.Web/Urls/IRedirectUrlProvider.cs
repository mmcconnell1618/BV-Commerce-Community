using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Urls
{
    public interface IRedirectUrlProvider
    {
        List<RedirectUrl> FindAll(int pageSize, int pageNumber);
        List<RedirectUrl> FindAllNotRedirected(int pageSize, int pageNumber);
                
        RedirectUrl FindById(long id);
        RedirectUrl FindByRequestedUrl(string requestedUrl);
        RedirectUrl Create(RedirectUrl u);
        bool Update(RedirectUrl u);
        bool Delete(long id);        
    }
}
