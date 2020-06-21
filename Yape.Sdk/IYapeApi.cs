using System.Threading.Tasks;
using Refit;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    [Headers(
        "x-channel: 1",
        "User-Agent: Dalvik/2.1.0 (Linux; U; Android 6.0; AX681 Build/MRA58K)"
    )]
    public interface IYapeApi
    {
        [Get("/identity/login/start")]
        Task LoginStart();
        [Get("/api/keyboard")]
        Task<Keyboard> KeyBoard();
        [Post("/api-mobile/public/identity/login/unlock")]
        Task<Identity> Login(UserLogin login);
        [Headers("Authorization: Bearer", "Content-Type: application/x-www-form-urlencoded")]
        [Post("/customers/v2/balance")]
        Task<Balance> Balance();
        [Headers("Authorization: Bearer")]
        [Get("/transfers/v2/history/{**filter}")]
        Task<History> History(string filter);
        [Headers("Authorization: Bearer")]
        [Get("/api-mobile/payments")]
        Task<OrderHistory> Orders();
        [Headers("Authorization: Bearer")]
        [Post("/api-mobile/payments")]
        Task<OrderResult> CreateOrder(Order order);
        [Headers("Authorization: Bearer")]
        [Get("/api-mobile/payments/{orderId}")]
        Task<OrderResult> Order(int orderId);
        [Headers("Authorization: Bearer")]
        [Delete("/api-mobile/payments/{orderId}")]
        Task<OrderResult> UndoOrder(int orderId);
        [Headers("Authorization: Bearer")]
        [Post("/api-mobile/transfers/targetUserData")]
        Task<Customer> Customer(CustomerPhone phone);
    }
}
