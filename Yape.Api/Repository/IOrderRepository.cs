using System.Threading.Tasks;
using Yape.Api.Models;

namespace Yape.Api.Repository
{
    public interface IOrderRepository
    {
        Task<OrderIntent> Get(string code);
        Task Save(string code, OrderIntent order);
    }
}
