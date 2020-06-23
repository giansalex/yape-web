using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Refit;
using Yape.Sdk.Entity;

namespace Yape.Sdk.Tests
{
    [Category("MANUAL")]
    public class YapeApiTests
    {
        private IYapeApi _api;
        private string _token;

        [SetUp]
        public void Setup()
        {
            var moq = new Mock<ITokenStore>();
            moq.Setup(m => m.Get()).ReturnsAsync(() => _token);
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(), UseCookies = true
            };
            //handler.Proxy = new WebProxy("127.0.0.1", 8888);

            var securityHandler = new SecurityHttpClientHandler(moq.Object, handler);
            var client = new HttpClient(securityHandler, true)
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
            var encryptor = new PinEncrypt();
            var pinText = await encryptor.GetPinText(keyboard.Response, pinPass.Select(c => c.ToString()).ToArray());
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