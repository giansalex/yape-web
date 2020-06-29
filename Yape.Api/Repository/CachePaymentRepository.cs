using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yape.Api.Models;

namespace Yape.Api.Repository
{
    public class CachePaymentRepository : IPaymentRepository
    {
        private const string PaymentPrefix = "yape-payment-";
        private readonly IDistributedCache _cache;

        public CachePaymentRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<PaymentIntent> Get(string code)
        {
            var json = await _cache.GetStringAsync(PaymentPrefix + code);

            return json == null ? null : JsonSerializer.Deserialize<PaymentIntent>(json);
        }

        public async Task Save(string code, PaymentIntent payment)
        {
            if (string.IsNullOrEmpty(payment.Id))
            {
                payment.Id = Guid.NewGuid().ToString();
                payment.State = OrderState.Pending;
            }

            var json = JsonSerializer.Serialize(payment);
            await _cache.SetStringAsync(PaymentPrefix + code, json);
        }
    }
}
