using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BVSoftware.CommerceDTO.v1.Client
{
    public class RestHelper
    {
        public static T GetRequest<T>(string uri)
            where T : class, new()
        {
            T result = SendGet<T>(uri);            
            return result;
        }
        public static T PostRequest<T>(string uri, string data)
            where T : class, new()
        {
            T result = SendWithData<T>(uri, "POST", data);
            return result;
        }
        public static T PutRequest<T>(string uri, string data)
            where T : class, new()
        {
            T result = SendWithData<T>(uri, "PUT", data);
            return result;
        }
        public static T DeleteRequest<T>(string uri, string data)
            where T : class, new()
        {
            T result = SendWithData<T>(uri, "DELETE", data);
            return result;
        }

        private const int DefaultTimeout = 100000;

        private static T SendGet<T>(string uri)
            where T : class, new()
        {
            try
            {
                string response = SendRequest(uri, "GET", null);
                T result = MerchantTribe.Web.Json.ObjectFromJson<T>(response);
                return result;
            }
            catch (Exception ex)
            {
                T result = new T();                
                if (typeof(T) is IApiResponse)
                {
                    ((IApiResponse)result).Errors.Add(new ApiError("EXCEPTION", ex.Message + " | " + ex.StackTrace));
                }                
                return new T();
            }
        }
        private static T SendWithData<T>(string uri, string method, string data)
            where T : class, new()
        {
            try
            {
                string response = SendRequest(uri, method, data);
                T result = MerchantTribe.Web.Json.ObjectFromJson<T>(response);
                return result;
            }
            catch (Exception ex)
            {
                T result = new T();
                if (typeof(T) is IApiResponse)
                {
                    ((IApiResponse)result).Errors.Add(new ApiError("EXCEPTION", ex.Message + " | " + ex.StackTrace));
                }
                return new T();
            }
        }
       
        private static string SendRequest(string serviceUrl, string method, string data)
        {
            return SendRequest(serviceUrl, method, data, null, DefaultTimeout);
        }
        private static string SendRequest(string serviceUrl, string method, string data, System.Net.WebProxy proxy, int timeout)
        {
            WebResponse objResp;
            WebRequest objReq;
            string strResp = string.Empty;
            byte[] byteReq;

            try
            {
                if (data == null)
                {
                    byteReq = null;
                }
                else
                {
                    byteReq = Encoding.UTF8.GetBytes(data);
                }
                objReq = WebRequest.Create(serviceUrl);
                objReq.Method = method.ToUpperInvariant();
                
                // Set Content Length If we Have Some
                if (byteReq != null)
                {
                    objReq.ContentLength = byteReq.Length;
                    objReq.ContentType = "application/x-www-form-urlencoded";
                }
                
                objReq.Timeout = timeout;
                if (proxy != null)
                {
                    objReq.Proxy = proxy;
                }
                
                // Send Data if we have some
                if (byteReq != null)
                {
                    Stream OutStream = objReq.GetRequestStream();
                    OutStream.Write(byteReq, 0, byteReq.Length);
                    OutStream.Close();
                }
                objResp = objReq.GetResponse();
                StreamReader sr = new StreamReader(objResp.GetResponseStream(), Encoding.UTF8, true);
                strResp += sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error SendRequest: " + ex.Message + " " + ex.Source);
            }

            return strResp;
        }

    }
}
