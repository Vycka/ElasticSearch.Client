using System;
using System.IO;
using System.Net;
using System.Text;

namespace ElasticSearch.Utils
{
    
    public class HttpRequest
    {
        #region Fields

        public readonly CookieContainer SessionCookies;
        private readonly string _baseUrl;
        
        #endregion

        #region Constructors

        // Disables SSL Verifications
        //static HttpRequest()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
        //}


        /// <param name="baseUrl">Base request uri used for relative requests</param>
        public HttpRequest(string baseUrl)
        {
            _baseUrl = baseUrl;

            SessionCookies = new CookieContainer();
        }

        #endregion

        #region Exposed Requests

        // GET
        public string MakeGetRequest(string relativeUrl)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "GET");
            return ExecuteWebRequest(httpRequest);
        }

        // GET Absolute
        public string MakeGetRequest(string baseUrl, string relativeUrl)
        {
            HttpWebRequest httpRequest = BuildAbsoluteBaseHttpWebRequest(new Uri(baseUrl + relativeUrl), "GET");
            return ExecuteWebRequest(httpRequest);
        }

        // POST
        public string MakePostJsonRequest(string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "POST");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        // POST Absolute
        public string MakePostJsonRequest(string baseUrl, string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildAbsoluteBaseHttpWebRequest(new Uri(baseUrl + relativeUrl), "POST");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        // PUT 
        public string MakePutJsonRequest(string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "PUT");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        #endregion

        #region Generic HTTP Request utils

        /// <summary>
        /// Executes Provided Http Request, But doesn't have ability to check response headers
        /// </summary>
        /// <param name="httpRequest">Http Request</param>
        /// <returns>Response Data</returns>
        private string ExecuteWebRequest(HttpWebRequest httpRequest)
        {
            WebResponse webResponse;
            return ExecuteWebRequest(httpRequest, out webResponse);
        }

        /// <summary>
        /// Executes Provided Http Request
        /// </summary>
        /// <param name="httpRequest">Http Request</param>
        /// <param name="webResponse">Response Headers</param>
        /// <returns>Response Data</returns>
        private string ExecuteWebRequest(HttpWebRequest httpRequest, out WebResponse webResponse)
        {
            string responseString;

            webResponse = httpRequest.GetResponse();

            Stream httpStream = webResponse.GetResponseStream();

            var httpResponseReader = new StreamReader(httpStream, Encoding.UTF8);
            responseString = httpResponseReader.ReadToEnd();
            httpResponseReader.Close();


            return responseString;
        }

        /// <summary>
        /// Inserts request stream into HttpRequest
        /// </summary>
        /// <param name="httpRequest">HttpRequest to which stream will be inserted</param>
        /// <param name="requestContent">Request Data</param>
        /// <param name="requestType">Type of Request</param>
        private void InsertSubmitRequestStream(HttpWebRequest httpRequest, string requestContent, SubmitRequestType requestType)
        {

            switch (requestType)
            {
                case SubmitRequestType.JSON:
                    httpRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                    httpRequest.ContentType = "application/json; charset=UTF-8";
                    break;

                case SubmitRequestType.X_WWW_FORM:
                    httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                    break;
            }

            byte[] requestBytes = Encoding.UTF8.GetBytes(requestContent);
            httpRequest.ContentLength = requestBytes.Length;

            var requestStream = httpRequest.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();
        }

        /// <summary>
        /// Builds HTTP Request base using RELATIVE URL and uses EnvironmentUrl As a URL Base
        /// </summary>
        /// <param name="relativeUrl">Relative URL</param>
        /// <param name="requestMethod">Request method (POST/GET/etc..)</param>
        private HttpWebRequest BuildRelativeBaseHttpWebRequest(string relativeUrl, string requestMethod)
        {
            var absoluteUrl = new Uri(_baseUrl + relativeUrl);
            return BuildAbsoluteBaseHttpWebRequest(absoluteUrl, requestMethod);
        }

        /// <summary>
        /// Builds HTTP Request base using provided ABSOLUTE Url Address
        /// </summary>
        /// <param name="absoluteUrl">Absolute URL</param>
        /// <param name="requestMethod">Request method (POST/GET/etc..)</param>
        private HttpWebRequest BuildAbsoluteBaseHttpWebRequest(Uri absoluteUrl, string requestMethod)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(absoluteUrl);
            httpRequest.Host = absoluteUrl.Host;
            httpRequest.Method = requestMethod;

            httpRequest.CookieContainer = SessionCookies;

            return httpRequest;
        }

        // ReSharper disable InconsistentNaming
        private enum SubmitRequestType
        {
            JSON,
            X_WWW_FORM

        }
        // ReSharper restore InconsistentNaming

        #endregion
    }
}
