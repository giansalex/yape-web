using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yape.Api.Models;

namespace Yape.Api.Repository
{
    public interface IPaymentRepository
    {
        Task<PaymentIntent> Get(string code);
        Task Save(string code, PaymentIntent payment);
    }
}
