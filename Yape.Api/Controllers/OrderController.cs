using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Yape.Api.Models;
using Yape.Api.Repository;
using Yape.Api.Services;
using Yape.Sdk;

namespace Yape.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("OrdersPolicy")]
    public class OrderController : ControllerBase
    {
        private readonly IYapeClient _yape;
        private readonly IOrderRepository _repository;

        public OrderController(IYapeClient yape, IOrderRepository repository)
        {
            _yape = yape;
            _repository = repository;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code, [FromHeader(Name = "Authorization")] string orderId)
        {
            var savedOrder = await _repository.Get(code);
            if (savedOrder == null || savedOrder.Id != orderId)
            {
                return NotFound();
            }

            if (savedOrder.State == OrderState.Complete)
            {
                return Ok(new
                {
                    Code = code,
                    savedOrder.State
                });
            }

            var history = await _yape.GetLatestHistory();

            var payment = history?.FirstOrDefault(h => h.Message == code);
            if (payment == null)
            {
                return Ok(new
                {
                    Code = code,
                    savedOrder.State
                });
            }

            if (payment.Amount != savedOrder.Amount)
            {
                return Ok(new
                {
                    Error = "Amount payed is invalid!"
                });
            }

            savedOrder.State = OrderState.Complete;
            await _repository.Save(code, savedOrder);

            return Ok(new
            {
                Code = code,
                savedOrder.State
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderIntent intent, [FromServices] OrderGenerator generator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCode = await generator.GenerateOrderCode();

            await _repository.Save(newCode, intent);

            return Ok(new
            {
                Code = newCode,
                intent.State,
                intent.Id
            });
        }
    }
}
