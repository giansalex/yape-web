using System.Linq;
using System.Threading.Tasks;
using Yape.Sdk.Entity;

namespace Yape.Sdk
{
    public class YapeAuthClient : IYapeClient
    {
        private readonly IYapeClient _client;
        private readonly IYapeApi _api;
        private readonly IPinResolver _pinResolver;
        private string[] _pinArray;

        public string UserId { get; set; }
        public string Email { get; set; }

        public string Pin
        {
            set { _pinArray = value.Select(c => c.ToString()).ToArray(); }
        }

        /// <summary>
        /// Token used for authenticated paths.
        /// </summary>
        public string TokenSaved { get; set; }

        public YapeAuthClient(IYapeClient client, IYapeApi api, IPinResolver pinResolver)
        {
            _client = client;
            _api = api;
            _pinResolver = pinResolver;
        }

        public async Task<decimal> GetBalance()
        {
            await VerifyLogin();

            return await _client.GetBalance();
        }

        public async Task<Payment[]> ListOrders()
        {
            await VerifyLogin();

            return await _client.ListOrders();
        }

        public async Task<HistoryItem[]> GetLatestHistory()
        {
            await VerifyLogin();

            return await _client.GetLatestHistory();
        }

        public async Task<Payment> CreateOrder(Order order)
        {
            await VerifyLogin();

            return await _client.CreateOrder(order);
        }

        public async Task<Payment> GetOrder(int orderId)
        {
            await VerifyLogin();

            return await _client.GetOrder(orderId);
        }

        public async Task<bool> DeleteOrder(int order)
        {
            await VerifyLogin();

            return await _client.DeleteOrder(order);
        }

        private async Task VerifyLogin()
        {
            var result = await _api.LoginStart();
            if (!result.Success) return;

            var keyboard = await _api.KeyBoard();
            if (!keyboard.Success) return;

            var pinText = await _pinResolver.GetPinText(keyboard.Response, _pinArray);
            var login = await _api.Login(new UserLogin
            {
                Uuid = UserId,
                Username = Email,
                PinText = new[] {pinText}
            });

            if (!login.Success) return;

            TokenSaved = login.Response.AuthToken.AccessToken;
        }
    }
}
