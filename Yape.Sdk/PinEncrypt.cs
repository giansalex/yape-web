using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yape.Sdk
{
    public class PinEncrypt : IPinResolver
    {
        private static Dictionary<string, string> _pines = new Dictionary<string, string>
        {
            {"0", "de98e1c36667"},
            {"1", "df99e0c26766"},
            {"2", "dc9ae3c16465"},
            {"3", "dd9be2c06564"},
            {"4", "da9ce5c76263"},
            {"5", "db9de4c66362"},
            {"6", "d89ee7c56061"},
            {"7", "d99fe6c46160"},
            {"8", "d690e9cb6e6f"},
            {"9", "d791e8ca6f6e"}
        };

        public Task<string> GetPinText(string[] keyboard, string[] pinPassword)
        {
            var indexKeys = new Queue<string>();
            foreach (var item in pinPassword)
            {
                var idx = Array.IndexOf(keyboard, item);
                indexKeys.Enqueue(idx.ToString());
            }

            var init = 0;
            var pinText = "";
            foreach (var pin in indexKeys)
            {
                pinText += _pines[pin].Substring(init, 2);
                init += 2;
            }

            return Task.FromResult(pinText);
        }
    }
}
