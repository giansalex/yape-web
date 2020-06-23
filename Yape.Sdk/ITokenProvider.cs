using System.Threading.Tasks;

namespace Yape.Sdk
{
    public interface ITokenProvider
    {
        Task<string> GetToken();
    }
}
