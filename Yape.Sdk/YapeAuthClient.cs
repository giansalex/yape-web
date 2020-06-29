using System;
using System.Collections.Generic;
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
        private readonly ITokenStore _store;
        private string[] _pinArray;
        private static DateTime _expire;

        public string UserId { get; set; }
        public string Email { get; set; }

        public string Pin
        {
            set { _pinArray = value.Select(c => c.ToString()).ToArray(); }
        }

        public YapeAuthClient(IYapeClient client, IYapeApi api, IPinResolver pinResolver, ITokenStore store)
        {
            _client = client;
            _api = api;
            _pinResolver = pinResolver;
            _store = store;
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

        public async Task<Customer> GetCustomer(string phone)
        {
            await VerifyLogin();

            return await _client.GetCustomer(phone);
        }

        public async Task<PhoneResult> VerifyContacts(IEnumerable<Contact> contacts)
        {
            await VerifyLogin();

            return await _client.VerifyContacts(contacts);
        }

        private async Task VerifyLogin()
        {
            var token = await _store.Get();
            if (!string.IsNullOrEmpty(token) && DateTime.Now < _expire)
            {
                return;
            }

            var result = await _api.LoginStart();
            if (!result.Success) throw new Exception("Cannot login for user " + Email);

            var keyboard = await _api.KeyBoard();
            if (!keyboard.Success) throw new Exception("Cannot login for user " + Email);

            var pinText = await _pinResolver.GetPinText(keyboard.Response, _pinArray);
            var login = await _api.Login(new UserLogin
            {
                Uuid = UserId,
                Username = Email,
                PinText = new[] {pinText}
            });

            if (!login.Success) throw new Exception("Cannot login for user " + Email);

            _expire = DateTime.Now.AddSeconds(login.Response.AuthToken.ExpiresIn * 0.8);

            await _store.Save(login.Response.AuthToken.AccessToken);
        }
    }
}
