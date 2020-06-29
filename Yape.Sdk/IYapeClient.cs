using System.Collections.Generic;
using System.Threading.Tasks;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    public interface IYapeClient
    {
        Task<decimal> GetBalance();
        
        Task<Payment[]> ListOrders();

        Task<HistoryItem[]> GetLatestHistory();

        Task<Payment> CreateOrder(Order order);

        Task<Payment> GetOrder(int orderId);

        Task<bool> DeleteOrder(int order);

        Task<Customer> GetCustomer(string phone);

        Task<PhoneResult> VerifyContacts(IEnumerable<Contact> contacts);
    }
}
