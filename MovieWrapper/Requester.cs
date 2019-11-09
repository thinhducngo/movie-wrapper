using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieWrapper
{
    public interface IRequester
    {
        Task<T> Get<T>(string requestUrl);
        Task<T> Post<T>(string requestUrl, object data);
        Task<T> Post<T>(string requestUrl, object data, string contentType);
    }

    /// <summary>
    /// REST serivce
    /// </summary>
    public class Requester : IRequester
    {
        public async Task<T> Get<T>(string requestUrl)
        {
            return await Request<T>(requestUrl, "GET");
        }

        public async Task<T> Post<T>(string requestUrl, object data)
        {
            return await Request<T>(requestUrl, "POST", data, "application/json");
        }

        public async Task<T> Post<T>(string requestUrl, object data, string contentType)
        {
            return await Request<T>(requestUrl, "POST", data, contentType);
        }

        private async Task<T> Request<T>(string requestUrl, string method, object data = null, string contentType = "application/json")
        {
            var responseFromServer = await Request(requestUrl, method, data, contentType);
            return JsonConvert.DeserializeObject<T>(responseFromServer);
        }

        private async Task<string> Request(string requestUrl, string method, object data, string contentType)
        {
            var request = WebRequest.Create(requestUrl);
            request.Method = method;
            request.ContentType = contentType;

            if (method != "GET")
            {
                byte[] byteArray;
                if (contentType == "application/x-www-form-urlencoded") byteArray = Encoding.UTF8.GetBytes(data as string);
                else byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                request.ContentLength = byteArray.Length;

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            string responseFromServer = null;
            using (var response = await request.GetResponseAsync())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = await reader.ReadToEndAsync();
                    }
                }
            }

            return responseFromServer;
        }
    }
}
