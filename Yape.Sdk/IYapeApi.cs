using System.Threading.Tasks;
using Refit;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    public interface IYapeApi
    {
        [Get("/identity/login/start")]
        Task LoginStart();
        [Get("/api/keyboard")]
        Task<Keyboard> KeyBoard();
        [Post("/api-mobile/public/identity/login/unlock")]
        Task<Identity> Login(UserLogin login);
        [Post("/customers/v2/balance")]
        Task<Balance> Balance();
        [Get("/transfers/v2/history/0/0/7")]
        Task<History> History();
        [Get("/api-mobile/payments")]
        Task<OrderHistory> Orders();
        [Post("/api-mobile/payments")]
        Task<OrderResult> Order(Order order);
        [Post("/api-mobile/transfers/targetUserData")]
        Task<Customer> Customer(CustomerPhone phone);
    }
}
