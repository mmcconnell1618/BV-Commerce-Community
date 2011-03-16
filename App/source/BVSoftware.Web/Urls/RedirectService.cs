using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Urls
{
    public class RedirectService
    {
        public IRedirectUrlProvider Provider { get; set; }

        public RedirectService(IRedirectUrlProvider provider)
        {
            Provider = provider;
        }

        public string FindRedirectForRequest(string requestedUrl)
        {
            RedirectUrl u = Provider.FindByRequestedUrl(requestedUrl.Trim().ToLowerInvariant());
            if (u != null)
            {
                return u.RedirectTo;
            }
            else
            {
                // doesn't exist, create it
                u = new RedirectUrl() { RequestedUrl = requestedUrl };
                Provider.Create(u);
                return string.Empty;
            }
        }
    }
}
