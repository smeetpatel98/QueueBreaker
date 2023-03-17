using Blazored.LocalStorage;
using QueueBreaker_UI.WASM.Contracts;
using QueueBreaker_UI.WASM.Models;
using System.Net.Http;

namespace QueueBreaker_UI.WASM.Service
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        public ItemRepository(HttpClient client,
            ILocalStorageService localStorage) : base(client, localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }
    }
}
