using System;
using System.Collections.Generic;
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

        public string Jwt { get; set; }
        
        private string Authorization { get; set; }
        
        // Public only for testing purposes
        public RequestData RequestData = new();
    
        public StrapiService(string backEndUrl)
        {
            if (string.IsNullOrEmpty(backEndUrl)) throw new ArgumentNullException(nameof(backEndUrl));
            BackEndUrl = backEndUrl;
        }

        /// <summary>
        /// Logs in to Strapi backend with username and password
        /// Throws an error if login is unsuccessful
        /// </summary>
        /// <param name="userName">Username passed through interface</param>
        /// <param name="password">Password passed through interface</param>
        /// <param name="loginInfo"></param>
        /// <returns>Returns True is login is successful</returns>
        public async Task<bool> Login<T>(T loginInfo) where T : Content
        {
            //string rawData = JsonConvert.SerializeObject(loginData);
            //StringContent outputData = new(rawData, Encoding.UTF8, "application/json");
            
            Uri url = new (BuildUrl("auth/local", ""));
            
            string contentString = loginInfo.ToString();
            
            StringContent outputData = BuildRequest(url, contentString);


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
        /// <param name="contentType">The type of the collection. This will be the plural form of the type. For example `customers` to retrieve a customer record.</param>
        /// <param name="id">Id of the record. If provided, the record with the matching id will be returned. When omitted, the entire collection will be returned.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Get<T>(string id) where T : Content
        {
            if(id == null)
                throw new Exception($"id: {nameof(id)} must be provided");
            
            
            Uri url = new (BuildUrl("", id));

            using HttpClient client = new();
            
            string result;
            
            using HttpRequestMessage request = new (HttpMethod.Get, url);
            
            SetRequestAuthorizationHeader(client);

            HttpResponseMessage response = await client.SendAsync(request);
            result = response.Content.ReadAsStringAsync().Result;

            //T content = JsonConvert.DeserializeObject<T>(result, new ContentConverter());
            
            return result;
        }

        /*public async Task<string> Get(string contentType, string id)
        {
            if (id == null)
                throw new Exception($"id: {nameof(id)} must be provided");


            Uri url = new(BuildUrl(contentType, id));

            using HttpClient client = new();

            string result;

            try
            {
                using HttpRequestMessage request = new(HttpMethod.Get, url);

                SetRequestAuthorizationHeader(client);

                HttpResponseMessage response = await client.SendAsync(request);
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Getting {contentType} / failed {e}");
                throw;
            }

            BuildDataForFiles test = new();

            string newTest = test.ConvertJsonToCsv(result, ",");
            test.WriteToCsvFile(newTest);
            

            return newTest;
            
        }*/

        /// <summary>
        /// Creates a new content record on Strapi.
        /// </summary>
        /// <param name="contentType">The type of the collection. This will be the plural form of the type. For example `customers` to retrieve a customer record.</param>
        /// <param name="data">Object that contains the data required to create a new record of `contentType` type.</param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> Create<T>(T content) where T : Content
        {
            if(content == null)
                throw new Exception($"contentType: {nameof(content)} must be provided");
            
            Uri url = new (BuildUrl(content.Name, ""));
            
            string contentString = content.ToString();
            
            //Content jsonObject = JsonConvert.DeserializeObject<Content>(contentString, new ContentConverter()) ?? throw new InvalidOperationException();
            StringContent outputData = BuildRequest(url, contentString);
            
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
        /// <param name="contentType">The type of the collection. This will be the plural form of the type. For example `customers` to retrieve a customer record.</param>
        /// <param name="data">Object that contains the updated fields of the record. It also needs to contain the `id` field to match the existing record.</param>
        /// <returns>True if request succeeds, false if request fails</returns>
        /// <exception cref="Exception"></exception>
        /*public async Task<bool> Update(string contentType, RequestData data)
        {
            if(contentType == null)
                throw new Exception($"contentType: {nameof(contentType)} must be provided");
            
            if(data.id == null)
                throw new Exception($"data: {nameof(data)} must have an ID");
            
            Uri url = new (BuildUrl(contentType, data.id.ToString()));
            
            StringContent request = BuildRequest(url, data);

            using HttpClient client = new();

            try
            {
                SetRequestAuthorizationHeader(client);
                
                await client.PutAsync(url, request);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Updating {contentType} / {data.id} failed {e}");
                return false;
            }

            return true;
        }*/
        
        /// <summary>
        /// Deletes a content record from Strapi.
        /// </summary>
        /// <param name="contentType">The type of the collection. This will be the plural form of the type. For example `customers` to retrieve a customer record.</param>
        /// <param name="id">Id of the record.</param>
        /// <returns>true if Delete is successful.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Del(string contentType, string id)
        {
            if(contentType == null)
                throw new Exception($"contentType: {nameof(contentType)} must be provided");
            
            if(id == null)
                throw new Exception($"id: {nameof(id)} must be provided");
            
            Uri url = new(BuildUrl(contentType, id));
            

            using HttpClient client = new();

            try
            {
                using HttpRequestMessage request = new (HttpMethod.Delete, url);
                SetRequestAuthorizationHeader(client);

                await client.SendAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Deleting {contentType} / {id} failed {e}");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Gets the data corresponding to a file that was previously uploaded.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <returns>The file.</returns>
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
        
        /*public async Task<string> UploadFile(string fileId, string fileName)
        {
            if(fileId == null)
                throw new Exception($"fileId: {nameof(fileId)} must be provided");
            
            if(fileName == null)
                throw new Exception($"fileName: {nameof(fileName)} must be provided");

            
            var url = new Uri(BuildUrl("upload/files", fileId));
            
            StringContent request = BuildRequest(url, data);

            using var client = new HttpClient();
            
            string result;
            
            try
            {
                var response = await client.PutAsync(url, request);
            
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Getting data of file {fileId} / failed {e}");
                throw;
            }
            
            return result;
        }*/
        
                
        /// <summary>
        /// Responsible for building the correctly formatted url to use for requests
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string BuildUrl(string contentType, string id)
        {
            string url = BackEndUrl + contentType;
            if (!string.IsNullOrEmpty(id))
            {
                url += "/" + id;
            }
            return url;
        }

        /// <summary>
        /// Handles building the requests to be sent
        /// The requests are converted to JSON and build as string format
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /*private StringContent BuildRequest<T>(Uri url, T data) where T : Content
        {
            if (url == null)
                throw new Exception($"url: {nameof(url)} must be provided");

            if (data == null)
                throw new Exception($"data: {nameof(data)} must be provided");

            /*if (!string.IsNullOrEmpty(data.jwt))
            {
                data.Authorization = data.jwt;
            }#1#
            
            //string rawData = JsonConvert.SerializeObject(data);
            
            string serializedMessage = JsonConvert.SerializeObject(data, new ContentConverter());
            
            StringContent outputData = new(serializedMessage, Encoding.UTF8, "application/json");

            return outputData;
        }*/
        
        private StringContent BuildRequest(Uri url, string data)
        {
            if (url == null)
                throw new Exception($"url: {nameof(url)} must be provided");

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
            //client.DefaultRequestHeaders.Add("page", "10");
        }
        
        
    }
}