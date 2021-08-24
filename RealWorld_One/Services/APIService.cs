using System.Net.Http;
using System.Threading.Tasks;
using RealWorld_One.Interfaces;

namespace RealWorld_One.Services
{
    public class APIService : IAPIService
    {
        public async Task<HttpResponseMessage> CallAPI(string URL)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetAsync(URL);
        }
    }
}
