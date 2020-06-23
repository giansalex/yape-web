using System.Threading.Tasks;

namespace Yape.Sdk
{
    public interface ITokenStore
    {
        Task<string> Get();

        Task Save(string token);
    }
}
