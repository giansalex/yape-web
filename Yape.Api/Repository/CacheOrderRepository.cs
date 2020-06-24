using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Yape.Api.Models;

namespace Yape.Api.Repository
{
    public class CacheOrderRepository : IOrderRepository
    {
        private const string OrderPrefix = "yape-order-";
        private readonly IDistributedCache _cache;

        public CacheOrderRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<OrderIntent> Get(string code)
        {
            var json = await _cache.GetStringAsync(OrderPrefix + code);

            return json == null ? null : JsonSerializer.Deserialize<OrderIntent>(json);
        }

        public async Task Save(string code, OrderIntent order)
        {
            if (string.IsNullOrEmpty(order.Id))
            {
                order.Id = Guid.NewGuid().ToString();
                order.State = OrderState.Pending;
            }

            var json = JsonSerializer.Serialize(order);
            await _cache.SetStringAsync(OrderPrefix + code, json);
        }
    }
}
