using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Urls.Providers
{
    public class MemoryProvider: IRedirectUrlProvider
    {

        private List<RedirectUrl> urls = new List<RedirectUrl>();

        public MemoryProvider()
        {
            urls.Add(new RedirectUrl() { Id = 0, RequestedUrl = "/accounts", RedirectTo = "/account" });
            urls.Add(new RedirectUrl() { Id = 1, RequestedUrl = "/changepassword", RedirectTo = "/account/changepassword" });
            urls.Add(new RedirectUrl() { Id = 2, RequestedUrl = "/changeemail", RedirectTo = "/account/changeemail" });
            urls.Add(new RedirectUrl() { Id = 3, RequestedUrl = "/paymentinformation", RedirectTo = "/account/paymentinformation" });
            urls.Add(new RedirectUrl() { Id = 4, RequestedUrl = "/company/contact.aspx", RedirectTo = "/company/contact" });
        }

        public void ClearUrls()
        {
            urls.Clear();
        }

        #region IRedirectUrlProvider Members

        public List<RedirectUrl> FindAll(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }
        public List<RedirectUrl> FindAllNotRedirected(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }
        public RedirectUrl FindById(long id)
        {
            throw new NotImplementedException();
        }

        public RedirectUrl FindByRequestedUrl(string requestedUrl)
        {            
            var r = (from url in urls
                     where url.RequestedUrl == requestedUrl
                     select url).SingleOrDefault();

            if (r != null)
            {
                return (RedirectUrl)r;
            }

            return null;                         
        }

        public RedirectUrl Create(RedirectUrl u)
        {
            RedirectUrl unow = FindByRequestedUrl(u.RequestedUrl);
            if (unow != null)
            {
                return unow;
            }
            else
            {
                u.Id = urls.Count;
                urls.Add(u);
                return u;
            }            
        }

        public bool Update(RedirectUrl u)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
