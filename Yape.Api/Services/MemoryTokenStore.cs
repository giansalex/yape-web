using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Yape.Sdk;

namespace Yape.Api.Services
{
    public class MemoryTokenStore : ITokenStore
    {
        private const string TokenKey = "yape-token";
        private readonly IDistributedCache _cache;

        public MemoryTokenStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> Get()
        {
            return await _cache.GetStringAsync(TokenKey);
        }

        public async Task Save(string token)
        {
            await _cache.SetStringAsync(TokenKey, token);
        }
    }
}
