using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yape.Sdk;
using Yape.Sdk.Entity;

namespace Yape.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IYapeClient _yape;

        public AdminController(IYapeClient yape)
        {
            _yape = yape;
        }

        [HttpGet("History")]
        public async Task<IEnumerable<HistoryItem>> GetHistory()
        {
            return await _yape.GetLatestHistory();
        }

        [HttpGet("Orders")]
        public async Task<IEnumerable<Payment>> GetOrders()
        {
            return await _yape.ListOrders();
        }

        [HttpGet("Customer/{phone:regex(^[[9]]\\d{{8}}$)}")]
        public async Task<Customer> GetCustomer(string phone)
        {
            return await _yape.GetCustomer(phone);
        }
    }
}
