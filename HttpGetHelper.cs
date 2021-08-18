using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GetCityApp
{
    public class HttpGetHelper
    {
        string url = string.Empty;
        public string Url
        {
            set { url = value; }
        }

        int timeOut = 10 * 1000;
        public int Timeout
        {
            set { timeOut = value; }
        }

        string contentType = "text/html;charset=utf-8";
        public string ContentType
        {
            set { contentType = value; }
        }

        string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36 ";
        public string UserAgent
        {
            set { userAgent = value; }
        }

        Encoding encode = Encoding.UTF8;
        public Encoding Encode
        {
            set { encode = value; }
        }
        string request_Method = "get";
        public string RequestMethod
        {
            set { request_Method = value; }
        }
        /// <summary>
        /// get html content
        /// </summary>
        /// <param name="cls">town=1;village=2</param>
        /// <param name="cookies">if cls=1 then ref cookies</param>
        /// <returns></returns>
        public string GetHtml(int cls, ref string cookies)
        {
            string html = string.Empty;
            try
            {
                if (url != string.Empty)
                {
                    HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = this.timeOut;
                    request.ContentType = this.contentType;
                    request.UserAgent = this.userAgent;
                    request.Headers.Add(HttpRequestHeader.Cookie, cookies);
                    request.Method = request_Method;
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {//如果是县级url，则记录cookie
                            if (cls == 1)
                            {
                                CookieCollection cookieCollection = response.Cookies;
                                foreach (Cookie item in cookieCollection)
                                {
                                    cookies = item.Name + "=" + item.Value + ";";
                                }
                                cookies.Remove(cookies.Length - 1);
                            }

                            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), encode))
                            {
                                html = streamReader.ReadToEnd();
                                streamReader.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception($"GetHtml失败，url:{url}");
            }
            return html;
        }
    }
}
