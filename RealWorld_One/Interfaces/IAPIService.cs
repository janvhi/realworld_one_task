using System.Net.Http;
using System.Threading.Tasks;

namespace RealWorld_One.Interfaces
{
    public interface IAPIService
    {
        Task<HttpResponseMessage> CallAPI(string URL);
    }
}
