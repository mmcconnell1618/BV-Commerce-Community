using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Urls
{
    public class RedirectUrl
    {
        public long Id { get; set; }
        public string RequestedUrl { get; set; }
        public string RedirectTo { get; set; }

        public RedirectUrl()
        {

        }
    }
}
