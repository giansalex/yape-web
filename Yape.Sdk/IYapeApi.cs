using System.Threading.Tasks;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    public interface IYapeApi
    {
        Task LoginStart();
        Task<Keyboard> KeyBoard();
        Task<Identity> Login(UserLogin login);
        Task<Balance> Balance();
        Task<History> History();
        Task<OrderResult> Order(Order order);
        Task<OrderHistory> Orders();
        Task<Customer> Customer(CustomerPhone phone);
    }
}
