using System;
using System.Threading.Tasks;
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

            var customer = await _yape.GetCustomer(intent.Phone);
            if (customer == null)
            {
                return Ok(new { Error = "Customer phone not found" });
            }

            var result = await _yape.CreateOrder(new Order
            {
                CashTag = customer.Cashtag,
                Amount = intent.Amount.ToString("F2"),
                Message = $"Paga tu pedido #{new Random().Next(1, 100)} Prueba"
            });

            if (result == null)
            {
                return Ok(new { Error = true });
            }

            var newCode = await generator.GenerateOrderCode();
            intent.Create = DateTime.Now;
            await _repository.Save(newCode, intent);

            return Ok(new
            {
                result.Id,
                result.CreationTime,
                result.Status
            });
        }
    }
}
