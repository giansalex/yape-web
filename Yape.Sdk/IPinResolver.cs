using System.Threading.Tasks;

namespace Yape.Sdk
{
    public interface IPinResolver
    {
        Task<string> GetPinText(string[] keyboard, string[] pinPassword);
    }
}
