using System;
using System.Threading.Tasks;
using Yape.Sdk;

namespace Yape.Api.Services
{
    public class MemoryTokenStore : ITokenStore
    {
        public Task<string> Get()
        {
            throw new NotImplementedException();
        }

        public Task Save(string token)
        {
            throw new NotImplementedException();
        }
    }
}
