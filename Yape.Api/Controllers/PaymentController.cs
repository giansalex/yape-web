using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Yape.Api.Models;
using Yape.Api.Repository;
using Yape.Api.Services;
using Yape.Sdk;
using Yape.Sdk.Entity;

namespace Yape.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("OrdersPolicy")]
    public class PaymentController : ControllerBase
    {
        private readonly IYapeClient _yape;
        private readonly IPaymentRepository _repository;

        public PaymentController(IYapeClient yape, IPaymentRepository repository)
        {
            _yape = yape;
            _repository = repository;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code, [FromHeader(Name = "Authorization")] string paymentId)
        {
            var savePayment = await _repository.Get(code);
            if (savePayment == null || savePayment.Id != paymentId)
            {
                return NotFound();
            }

            if (savePayment.State != OrderState.Pending)
            {
                return Ok(new
                {
                    Code = code,
                    savePayment.State
                });
            }

            var result = await _yape.GetOrder(savePayment.PaymentYapeId);
            if (result == null)
            {
                return Ok(new
                {
                    Error = "Payment not found!"
                });
            }
            
            savePayment.CompleteDate = DateTime.Now;
            savePayment.State = result.Status;
            await _repository.Save(code, savePayment);

            return Ok(new
            {
                Code = code,
                savePayment.State
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentIntent intent, [FromServices] OrderGenerator generator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCode = await generator.GenerateOrderCode();

            var verifyResult = await _yape.VerifyContacts(new []{ new Contact {Name = $"User-{newCode}", Phone = intent.Phone } });
            if (verifyResult == null || verifyResult.Phones.Length == 0)
            {
                return Ok(new { Error = "Customer phone not found" });
            }

            var result = await _yape.CreateOrder(new Order
            {
                CashTag = verifyResult.Phones.Single(),
                Amount = intent.Amount.ToString("F2"),
                Message = $"Paga tu pedido #{newCode} - Prueba"
            });

            if (result == null)
            {
                return Ok(new { Error = true });
            }

            intent.Create = DateTime.Now;
            intent.PaymentYapeId = result.Id;
            await _repository.Save(newCode, intent);

            return Ok(new
            {
                Code = newCode,
                intent.Id,
                intent.State
            });
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code, [FromHeader(Name = "Authorization")] string paymentId)
        {
            var savePayment = await _repository.Get(code);
            if (savePayment == null || savePayment.Id != paymentId)
            {
                return NotFound();
            }

            if (savePayment.State == OrderState.Canceled)
            {
                return Ok(new
                {
                    Code = code,
                    savePayment.State
                });
            }

            var success = await _yape.DeleteOrder(savePayment.PaymentYapeId);
            if (!success)
            {
                return Ok(new { Result = "Payment cannot delete." });
            }

            savePayment.CompleteDate = DateTime.Now;
            savePayment.State = OrderState.Canceled;
            await _repository.Save(code, savePayment);

            return Ok(new
            {
                savePayment.Id,
                savePayment.CompleteDate,
                savePayment.State
            });
        }
    }
}
