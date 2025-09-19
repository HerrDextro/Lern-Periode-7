using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    internal class APIRequestHelper
    {
        private static readonly HttpClient client = new HttpClient();
        
        public async static Task<HttpResponseMessage> PostCard(Request payload, string url)
        {
            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);
            return response;
        }

        public async static Task<HttpResponseMessage> GetCard(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            return response;
        }

        public async static Task<HttpResponseMessage> UpdateCard(string url)
        {
            HttpResponseMessage response = await client.PutAsync(url, new StringContent("",Encoding.UTF8,"application/json"));
            return response;
        }

    }
}
