using Blazored.LocalStorage;
using QueueBreaker_UI.WASM.Contracts;
using QueueBreaker_UI.WASM.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Service
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;

        public OrderRepository(HttpClient client,
            ILocalStorageService localStorage) : base(client, localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

       
    }
}
