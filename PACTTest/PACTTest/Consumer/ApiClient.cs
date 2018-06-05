using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PACTTest
{
    public class ApiClient 
    {
        public string BaseUri { get; set; }

        public ApiClient(string baseUri = null)
        {
            BaseUri = baseUri ?? "https://jsonplaceholder.typicode.com";
        }

        public Object GetResponse(string id)
        {
            string reasonPhrase;

        
        
            using (var client = new HttpClient { BaseAddress = new Uri(BaseUri) })
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/posts");

               // request.Headers.Add("Content-Type", "application/json");

                request.Content = new StringContent("Content-Type",
                                     Encoding.UTF8,
                                     "application/json");//CONTENT-TYPE header

                var response = client.SendAsync(request);

                var content = response.Result.Content.ReadAsStringAsync().Result;
                var status = response.Result.StatusCode;

                reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

 

                if (status == HttpStatusCode.OK)
                {
                    return !String.IsNullOrEmpty(content) ?
                      JsonConvert.DeserializeObject<Object>(content)
                      : null;
                }

                return status;

                throw new Exception(reasonPhrase);
            }
        }

       
    }
}
