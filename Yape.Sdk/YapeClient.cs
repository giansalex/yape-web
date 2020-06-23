using System.Threading.Tasks;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    public class YapeClient : IYapeClient
    {
        private readonly IYapeApi _api;

        public YapeClient(IYapeApi api)
        {
            _api = api;
        }

        public async Task<decimal> GetBalance()
        {
            var result = await _api.Balance();

            return result.Success ? decimal.Parse(result.Response.Amount) : 0;
        }

        public async Task<Payment[]> ListOrders()
        {
            var result = await _api.Orders();

            return result.Success ? result.Response : null;
        }

        public async Task<HistoryItem[]> GetLatestHistory()
        {
            var result = await _api.History("0/0/7");

            return result.Success ? result.Response.Entities : null;
        }

        public async Task<Payment> CreateOrder(Order order)
        {
            var result = await _api.CreateOrder(order);

            return result.Success ? result.Response : null;
        }

        public async Task<Payment> GetOrder(int orderId)
        {
            var result = await _api.Order(orderId);

            return result.Success ? result.Response : null;
        }

        public async Task<bool> DeleteOrder(int order)
        {
            var result = await _api.UndoOrder(order);

            return result.Success;
        }

        public async Task<Customer> GetCustomer(string phone)
        {
            var result = await _api.Customer(new CustomerPhone
            {
                Cashtag = phone
            });

            return result.Success ? result.Response : null;
        }
    }
}
