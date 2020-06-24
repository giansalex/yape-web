using System;
using System.Threading.Tasks;
using Yape.Api.Models;
using Yape.Api.Repository;

namespace Yape.Api.Services
{
    public class OrderGenerator
    {
        private readonly IOrderRepository _orderRepository;

        public OrderGenerator(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<string> GenerateOrderCode()
        {
            OrderIntent order;
            string code;
            do
            {
                code = new Random().Next(1111, 9999).ToString();

                order = await _orderRepository.Get(code);
            } while (order != null);

            return code;
        }
    }
}
