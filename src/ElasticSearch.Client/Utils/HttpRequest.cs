using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace ElasticSearch.Client.Utils
{
    public class HttpRequest
    {
        #region Properties

        public readonly CookieContainer SessionCookies;
        private readonly string _baseUrl;

        private string _requestData;

        #endregion

        #region Constructors

        // Disables SSL Verifications
        static HttpRequest()
        {
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">Base request uri used for relative requests</param>
        public HttpRequest(string baseUrl)
        {
            if (baseUrl == null) 
                throw new ArgumentNullException("baseUrl");

            _baseUrl = baseUrl;

            SessionCookies = new CookieContainer();
        }

        #endregion

        #region Exposed Requests

        public string MakeRequest(string relativeUrl, RequestType requestType)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, requestType.ToString().ToUpperInvariant());
            return ExecuteWebRequest(httpRequest);
        }

        public string MakeGetRequest(string relativeUrl)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "GET");
            return ExecuteWebRequest(httpRequest);
        }

        public string MakeGetRequest(string baseUrl, string relativeUrl)
        {
            HttpWebRequest httpRequest = BuildAbsoluteBaseHttpWebRequest(new Uri(baseUrl + relativeUrl), "GET");
            return ExecuteWebRequest(httpRequest);
        }

        public string MakePostJsonRequest(string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "POST");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        public string MakePostJsonRequest(string baseUrl, string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildAbsoluteBaseHttpWebRequest(new Uri(baseUrl + relativeUrl), "POST");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        public string MakePutJsonRequest(string baseUrl, string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildAbsoluteBaseHttpWebRequest(new Uri(baseUrl + relativeUrl), "PUT");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        public string MakePutJsonRequest(string relativeUrl, string jsonContent)
        {
            HttpWebRequest httpRequest = BuildRelativeBaseHttpWebRequest(relativeUrl, "PUT");
            InsertSubmitRequestStream(httpRequest, jsonContent, SubmitRequestType.JSON);
            return ExecuteWebRequest(httpRequest);
        }

        #endregion

        #region Generic HTTP Request utils

        /// <summary>
        /// Executes Provided Http Request
        /// </summary>
        /// <param name="httpRequest">Http Request</param>
        /// <returns>Response Data</returns>
        private string ExecuteWebRequest(HttpWebRequest httpRequest)
        {
            PrintRequestInformation(httpRequest);

            string responseString;

            try
            {
                using (var webResponse = httpRequest.GetResponse())
                using (Stream httpStream = webResponse.GetResponseStream())
                using (var httpResponseReader = new StreamReader(httpStream, Encoding.UTF8))
                {
                    responseString = httpResponseReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Stream responseStream = ex.Response == null ? null : ex.Response.GetResponseStream();
                string response = "";
                if (responseStream != null)
                {
                    response = new StreamReader(responseStream).ReadToEnd();
                }

                // If no EventNum found, its still error, throw extended exception, which includes response stream.
                if (ex.Response == null)
                {
                    throw new ExtendedWebException(ex.Message, httpRequest.Address.ToString(), _requestData, response, ex);
                }
                else
                {
                    throw new ExtendedWebException(ex.Message, ex.Response.ResponseUri.ToString(), _requestData, response, ex);
                }
            }
            finally
            {
                _requestData = null;
            }

            return responseString;
        }

        private void PrintRequestInformation(HttpWebRequest httpRequest)
        {
            Debug.WriteLine(" # API {0} [{1}]", httpRequest.Method, httpRequest.Address);
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

            _requestData = requestContent;

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

        #region ToString()

        public override string ToString()
        {
            return _baseUrl;
        }

        #endregion
    }

    public enum RequestType
    {
        Get,
        Post,
        Put,
        Delete
    }

    public class ExtendedWebException : Exception
    {
        private readonly string _requestUrl;
        public readonly string WebResponse;
        private readonly string _requestData;

        public ExtendedWebException(string message, string requestUrl, string requestData, string webResponse, Exception innerException)
            : base(message, innerException)
        {
            if (requestUrl == null)
                throw new ArgumentNullException("requestUrl");

            _requestUrl = requestUrl;
            WebResponse = webResponse;
            _requestData = requestData ?? "NULL";
        }

        public override string Message
        {
            get
            {
                return string.Concat(
                    "\r\nMessage: ", base.Message,
                    "\r\nResponse URL:  ", _requestUrl,
                    "\r\nRequest:       ", _requestData,
                    "\r\nResponse:      ", WebResponse);
            }
        }

        public override string ToString()
        {
            return string.Concat(base.ToString(),
                "\r\nResponse URL: ", _requestUrl,
                "\r\nRequest:      ", _requestData,
                "\r\nResponse:     ", WebResponse);
        }
    }
}
