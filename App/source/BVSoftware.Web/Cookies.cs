using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class Cookies
    {
        public static string GetCookieString(string cookieName, System.Web.HttpContextBase context, Logging.ILogger log)
        {
            string result = string.Empty;

            try
            {
                if (context != null)
                {
                    if (context.Request != null)
                    {
                        if (context.Request.Browser.Cookies == true)
                        {
                            System.Web.HttpCookie checkCookie = context.Request.Cookies[cookieName];
                            if (checkCookie != null)
                            {
                                result = checkCookie.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogException(ex);
            }

            return result;
        }
        public static long GetCookieLong(string cookieName, System.Web.HttpContextBase context, Logging.ILogger log)
        {
            long result = -1;
            string temp = GetCookieString(cookieName, context, log);
            long.TryParse(temp, out result);
            return result;
        }
        public static Guid? GetCookieGuid(string cookieName, System.Web.HttpContextBase context, Logging.ILogger log)
        {
            Guid? result = null;
            string temp = GetCookieString(cookieName, context, log);
            try
            {
                result = new System.Guid(temp);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public static void SetCookieString(string cookieName, string value, System.Web.HttpContextBase context, bool temporary, Logging.ILogger log)
        {
            SetCookieString(cookieName, value, context, temporary, null,log);
        }
        public static void SetCookieStringWithDomain(string cookieName, string value, System.Web.HttpContextBase context, bool temporary, string domain, Logging.ILogger log)
        {
            SetCookieString(cookieName, value, context, temporary, null, domain,log);
        }
        public static void SetCookieString(string cookieName, string value, System.Web.HttpContextBase context, bool temporary, DateTime? expirationDate, Logging.ILogger log)
        {
            SetCookieString(cookieName, value, context, temporary, expirationDate, "",log);
        }
        public static void SetCookieString(string cookieName, string value, System.Web.HttpContextBase context, bool temporary, DateTime? expirationDate, string domain, Logging.ILogger log)
        {
            try
            {
                if (context != null)
                {
                    if (context.Request != null)
                    {
                        if (context.Request.Browser.Cookies == true)
                        {
                            System.Web.HttpCookie saveCookie = new System.Web.HttpCookie(cookieName, value);
                            if (!temporary)
                            {
                                if (expirationDate.HasValue)
                                {
                                    saveCookie.Expires = expirationDate.Value;
                                }
                                else
                                {
                                    saveCookie.Expires = DateTime.Now.AddYears(50);
                                }
                            }
                            if (domain.Trim().Length > 0)
                            {
                                saveCookie.Domain = domain;
                            }
                            context.Response.Cookies.Add(saveCookie);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogException(ex);
            }
        }
        public static void SetCookieLong(string cookieName, long value, System.Web.HttpContextBase context, bool temporary, Logging.ILogger log)
        {
            SetCookieString(cookieName, value.ToString(), context, temporary,log);
        }
        public static void SetCookieGuid(string cookieName, Guid value, System.Web.HttpContextBase context, bool temporary, Logging.ILogger log)
        {
            SetCookieString(cookieName, value.ToString(), context, temporary,log);
        }
    }
}
