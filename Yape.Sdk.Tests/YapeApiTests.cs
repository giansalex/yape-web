using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Refit;
using Yape.Sdk.Entity;

namespace Yape.Sdk.Tests
{
    [Category("MANUAL")]
    public class YapeApiTests
    {
        private static Dictionary<string, string> _pines = new Dictionary<string, string>
        {
            {"0", "de98e1c36667"},
            {"1", "df99e0c26766"},
            {"2", "dc9ae3c16465"},
            {"3", "dd9be2c06564"},
            {"4", "da9ce5c76263"},
            {"5", "db9de4c66362"},
            {"6", "d89ee7c56061"},
            {"7", "d99fe6c46160"},
            {"8", "d690e9cb6e6f"},
            {"9", "d791e8ca6f6e"}
        };
        private IYapeApi _api;
        private string _token;

        [SetUp]
        public void Setup()
        {
            var handler = new SecurityHttpClientHandler(() => Task.FromResult(_token))
            {
                CookieContainer = new CookieContainer(), UseCookies = true
            };
            //handler.Proxy = new WebProxy("127.0.0.1", 8888);
            var client = new HttpClient(handler, true)
            {
                BaseAddress = new Uri("https://yapesec.innovacxionbcp.com")
            };

            var settings = new RefitSettings(new SystemTextJsonContentSerializer());
            _api = RestService.For<IYapeApi>(client, settings);
        }

        [Test]
        [TestCaseSource(nameof(_yapeData))]
        public async Task CompleteTest(string id, string username, string pinPass)
        {
            var result = await _api.LoginStart();
            Assert.True(result.Success);
            var keyboard = await _api.KeyBoard();
            Assert.True(keyboard.Success);
            var pinText = PinEncrypt.GetPinHash(keyboard.Response, pinPass.Select(c => c.ToString()).ToArray());
            var identity = await _api.Login(new UserLogin
            {
                PinText = new []{ pinText},
                Username = username,
                Uuid = id
            });
            Assert.True(identity.Success);

            _token = identity.Response.AuthToken.AccessToken;

            var history = await _api.History("0/0/7");
            Assert.True(history.Success);
            foreach (var entity in history.Response.Entities)
            {
                var date = DateTimeOffset.FromUnixTimeSeconds(entity.Time / 1000)
                    .DateTime.ToLocalTime();
                var isOut = entity.Type == "OUT";
                var line = $"Date: {date:dd/MM/yyyy} {(isOut ? "To: " + entity.ContactTo.DisplayName : "From: " + entity.ContactFrom.DisplayName)} - Amount: S/ {entity.Amount:F2} | {entity.Message}";
                await TestContext.Out.WriteLineAsync(line);
            }

            var balance = await _api.Balance();
            Assert.True(balance.Success);
            var saldo = decimal.Parse(balance.Response.Amount);

            await TestContext.Out.WriteLineAsync($"Saldo S/.{saldo:F2}");

            var customer = await _api.Customer(new CustomerPhone {Cashtag = "943320216"});
            Assert.IsTrue(customer.Success);

            await TestContext.Out.WriteLineAsync("Cliente: " + customer.Response.Name);

            var order = await _api.CreateOrder(new Order
            {
                Amount = "5.00",
                CashTag = "947392421",
                Message = "#4444"
            });
            Assert.True(order.Success);
            await TestContext.Out.WriteLineAsync($"Payment: {order.Response.Id} | Status: {order.Response.Status}");

            var orders = await _api.Orders();
            Assert.True(orders.Success);

            foreach (var payment in orders.Response)
            {
                var line = $"Name: {payment.Name} , Amount: {payment.Amount:F2} , Status: {payment.Status}";
                await TestContext.Out.WriteLineAsync(line);
            }

            result = await _api.UndoOrder(order.Response.Id);
            Assert.True(result.Success);
        }

        private static object[] _yapeData =
        {
            new [] { "xxxxxxxxxxx", "xxx@xxxx.xx", "xxxxxx" },
        };
    }
}