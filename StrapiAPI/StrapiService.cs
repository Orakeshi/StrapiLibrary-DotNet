using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solarflare.StrapiAPI.StrapiRequests;

namespace Solarflare.StrapiAPI
{
    /// <summary>
    /// Allows for communication between user and Strapi backend
    /// </summary>
    public class StrapiService
    {
        private string BackEndUrl { get; set; }

        private string Jwt { get; set; }
        
        private string Authorization { get; set; }

        public StrapiService(string backEndUrl)
        {
            if (string.IsNullOrEmpty(backEndUrl)) throw new ArgumentNullException(nameof(backEndUrl));
            BackEndUrl = backEndUrl;
        }

        /// <summary>
        /// Logs in to Strapi backend with username and password
        /// Throws an error if login is unsuccessful
        /// </summary>
        /// <param name="loginInfo">Content class with username and password attributes</param>
        /// <returns>Returns True is login is successful</returns>
        public async Task<bool> Login<T>(T loginInfo) where T : Content
        {
            //string rawData = JsonConvert.SerializeObject(loginData);
            //StringContent outputData = new(rawData, Encoding.UTF8, "application/json");
            
            Uri url = new (BuildUrl("auth/local", ""));
            
            string contentString = loginInfo.ToString();
            
            StringContent outputData = BuildRequest(contentString);


            using HttpClient client = new ();

            string result;
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, outputData);
            
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            Jwt = JObject.Parse(result)["jwt"]?.ToString() ?? throw new InvalidOperationException();
            Authorization = Jwt;

            return true;
        }

        /// <summary>
        /// Logs in to Strapi backend with username and password
        /// Throws an error if login is unsuccessful
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Returns True is login is successful</returns>
        public async Task<bool> Login(string userName, string password)
        {
            Uri url = new (BuildUrl("auth/local", ""));

            LoginEntry loginInfo = new()
            {
                Identifier = userName,
                Password = password
            };
        
            string contentString = loginInfo.ToString();
            
            StringContent outputData = BuildRequest(contentString);


            using HttpClient client = new ();

            string result;
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, outputData);
            
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            Jwt = JObject.Parse(result)["jwt"]?.ToString() ?? throw new InvalidOperationException();
            Authorization = Jwt;

            return true;
        }

        /// <summary>
        /// Retrieves a content record from strapi
        /// </summary>
        /// <param name="content">content class with id is to be provided. If no id, all data will be returned</param>
        /// <returns>string format of the record</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Get<T>(T content) where T : Content
        {
            if(content == null)
                throw new Exception($"content: {nameof(content)} must be provided");
            
            string contentString = content.ToString();
            
            Uri url = new (BuildUrl(content.Name, JObject.Parse(contentString)["id"]?.ToString() ?? ""));

            using HttpClient client = new();

            string result;
            try
            {
                SetRequestAuthorizationHeader(client);
                
                HttpResponseMessage response = await client.GetAsync(url);
            
                result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Getting {content} / failed {e}");
                return $"{e}";
            }

            return result;
        }

        /// <summary>
        /// Creates a new content record on Strapi.
        /// </summary>
        /// <param name="content">content, of type class Content, is passed as param with data corresponding to each property</param>
        /// <returns>The id of the record created, is returned</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Create<T>(T content) where T : Content
        {
            if(content == null)
                throw new Exception($"contentType: {nameof(content)} must be provided");
            
            Uri url = new (BuildUrl(content.Name, ""));
            
            string contentString = content.ToString();

            //Content jsonObject = JsonConvert.DeserializeObject<Content>(contentString, new ContentConverter()) ?? throw new InvalidOperationException();
            StringContent outputData = BuildRequest(contentString);
            
            using HttpClient client = new();

            string result;
            try
            {
                SetRequestAuthorizationHeader(client);
                
                HttpResponseMessage response = await client.PostAsync(url, outputData);
            
                result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Getting {content} / failed {e}");
                return $"{e}";
            }
            //Return ID of content posted
            string contentId = JObject.Parse(result)["id"]?.ToString() ?? throw new InvalidOperationException();

            return contentId;
        }

        /// <summary>
        /// Updates an already existing content record on Strapi.
        /// </summary>
        /// <param name="content">content, of type class Content, is passed as param</param>
        /// <returns>returns the response from the request as string</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Update<T>(T content) where T : Content
        {
            if(content == null)
                throw new Exception($"content: {nameof(content)} must be provided");

            string contentString = content.ToString();
            
            if(JObject.Parse(contentString)["id"]?.ToString() == null)
                throw new Exception($"Method: {nameof(content)} must have an ID");

            Uri url = new (BuildUrl(content.Name, JObject.Parse(contentString)["id"]?.ToString() ?? throw new InvalidOperationException()));

            StringContent outputData = BuildRequest(contentString);

            using HttpClient client = new();

            string result;
            
            try
            {
                SetRequestAuthorizationHeader(client);
                
                HttpResponseMessage response = await client.PutAsync(url, outputData);
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Updating {content} / failed {e}");
                return $"{e}";
            }

            return result;
        }

        /// <summary>
        /// Deletes a content record from Strapi.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>string of the request result is returned</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Del(string id)
        {
            if(id == null)
                throw new Exception($"contentType: {nameof(id)} must be provided");

            Uri url = new (BuildUrl("prize-draw-entries", id));

            string result;

            using HttpClient client = new();

            try
            {
                SetRequestAuthorizationHeader(client);

                HttpResponseMessage response = await client.DeleteAsync(url);
                
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Deleting {id} / failed {e}");
                return $"{e}";
            }

            return result;
        }
        
        /// <summary>
        /// Gets the data, as a string, corresponding to a file that was previously uploaded.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <returns>String containing all the file data</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetFile(string fileId)
        {
            if(fileId == null)
                throw new Exception($"fileId: {nameof(fileId)} must be provided");
            
            Uri url = new(BuildUrl("upload/files", fileId));
            
            using HttpClient client = new();
            
            string result;
            
            try
            {
                using HttpRequestMessage request = new (HttpMethod.Get, url);
                
                SetRequestAuthorizationHeader(client);

                HttpResponseMessage response = await client.SendAsync(request);
                result = response.Content.ReadAsStringAsync().Result;
            }
            
            catch (Exception e)
            {
                Console.WriteLine($"Getting data of file {fileId} / failed {e}");
                throw;
            }

            return result;
        }
        
        /// <summary>
        /// Responsible for building the correctly formatted url to use for requests
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string BuildUrl(string contentType, string id)
        {
            string url = BackEndUrl + contentType/*+"?_start=3000&_limit=100"*/;
            if (!string.IsNullOrEmpty(id))
            {
                url += "/" + id;
            }
            return url;
        }

        /// <summary>
        /// Responsible for building string content data that can be sent via the request
        /// </summary>
        /// <param name="data">string of json format which contains the data to be sent</param>
        /// <returns>returns string content data</returns>
        /// <exception cref="Exception"></exception>
        private StringContent BuildRequest(string data)
        {
            if (data == null)
                throw new Exception($"data: {nameof(data)} must be provided");
            
            JObject jsonObject = JObject.Parse(data);
            
            StringContent outputData = new(jsonObject.ToString(), Encoding.UTF8, "application/json");

            return outputData;
        }
        
        /// <summary>
        /// Responsible for assigning Authorization header to each request
        /// </summary>
        /// <param name="client"></param>
        private void SetRequestAuthorizationHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Jwt);
        }
    }
}