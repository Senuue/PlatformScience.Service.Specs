using System;
using System.Text;
using System.Net;
using System.Text.Json;
using System.IO;

namespace PlatformScience.Service.Specs.Helpers
{
    public static class RestTester
    {
        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 229);
        }

        public static T0 GetResponse<T0>(this HttpWebResponse response)
        {
            using (var newStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(newStream))
                {
                    var result = reader.ReadToEnd();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    return JsonSerializer.Deserialize<T0>(result,options);
                }
            }              
        }

        public static HttpWebResponse GetApiWebResponse<T1>(Verb verb, string requestEndPoint, T1 body)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
                        
            var deserializedBody = body == null ? string.Empty : JsonSerializer.Serialize(body,options);

            if (string.IsNullOrEmpty(requestEndPoint))
            {
                throw new InvalidOperationException("Request Endpoint is not valid");
            }

            try
            {
                if (WebRequest.Create(new Uri(requestEndPoint)) is HttpWebRequest webRequest)
                {
                    webRequest.Method = verb.ToString().ToUpperInvariant();
                    webRequest.ContentType = "application/json";
                    webRequest.Accept = "application/json";
                    if (verb is Verb.Post)
                    {
                        webRequest.ContentLength = deserializedBody?.Length ?? 0;
                    }

                    if (!string.IsNullOrEmpty(deserializedBody))
                    {
                        var postData = Encoding.UTF8.GetBytes(deserializedBody);

                        using (var requestStream = webRequest.GetRequestStream())
                        {
                            if (requestStream is null)
                            {
                                throw new WebException();
                            }

                            requestStream.Write(postData, 0, postData.Length);
                        }
                    }

                    if (webRequest.GetResponse() is HttpWebResponse httpWebResponse)
                    {
                        return httpWebResponse;
                    }
                }
                return null;
            }
            catch (UriFormatException)
            {
                throw new InvalidOperationException("An error occured with the request end point");
            }
            catch (WebException e)
            {
                if (e.Response is null)
                {
                    throw new WebException($"An error occured: {verb} at {requestEndPoint} failed with error: {e.Message}");
                }

                if (e.Response is HttpWebResponse httpWebResponse)
                {
                    return httpWebResponse;
                }

                return null;
            }
        }
    }
}

