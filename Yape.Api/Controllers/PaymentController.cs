﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yape.Api.Models;
using Yape.Sdk;
using Yape.Sdk.Entity;

namespace Yape.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IYapeClient _yape;

        public PaymentController(IYapeClient yape)
        {
            _yape = yape;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _yape.GetOrder(id);
            
            return Ok(new
            {
                result.Id,
                result.CreationTime,
                result.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderIntent intent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _yape.CreateOrder(new Order
            {
                CashTag = intent.Phone,
                Amount = intent.Amount.ToString("F2"),
                Message = $"Paga tu pedido #{new Random().Next(1, 100)} Prueba"
            });

            return Ok(new
            {
                result.Id,
                result.CreationTime,
                result.Status
            });
        }
    }
}
