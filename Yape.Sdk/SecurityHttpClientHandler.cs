using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Yape.Sdk
{
    public class SecurityHttpClientHandler : HttpClientHandler
    {
        private static Dictionary<string, string> _headers = new Dictionary<string, string>();
        private static Dictionary<string, string> _mapAuth = new Dictionary<string, string>
        {
            {"MTE=", "MjgzNA=="},
            {"Nw==", "MTMxOQ=="},
            {"MjI=", "MzAyOQ=="},
            {"OA==", "MjA2Mg=="},
            {"MTg=", "MzExMA=="},
            {"NA==", "NTcy"},
            {"MjA=", "Mjc4OA=="},
            {"MTA=", "Mjg2Mw=="},
            {"Mg==", "MTI4"},
            {"NQ==", "MzAxOA=="},
            {"MjM=", "Mzc2OQ=="},
            {"MjE=", "MjkxNg=="},
            {"OQ==", "Mzc5Mg=="},
            {"MTM=", "MjU0OA=="},
            {"MTY=", "MzAzOA=="}
        };

        private readonly Func<Task<string>> _getToken;

        public SecurityHttpClientHandler(Func<Task<string>> getToken)
        {
            _getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                var token = await _getToken().ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }

            LoadHeaders(request);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                SaveHeaders(response);
            }

            return response;
        }

        static void LoadHeaders(HttpRequestMessage request)
        {
            foreach (var item in _headers)
            {
                var value = item.Key == "x-validate-auth" ? DecodeNextAuth(item.Value) : item.Value;
                request.Headers.Add(item.Key, value);
            }
            request.Headers.Add("Accept", "application/json");
        }

        static void SaveHeaders(HttpResponseMessage response)
        {
            string[] keys = {"x-validate-auth", "X-KEYBOARD"};

            foreach (var key in keys)
            {
                if (response.Headers.TryGetValues(key, out var values)) {
                    if (_headers.ContainsKey(key)) {
                        _headers[key] = values.First();
                    } else {
                        _headers.Add(key, values.First());
                    }
                }
            }
        }

        static string DecodeNextAuth(string header)
        {
            var authParts = header.Split(':');
            if (!_mapAuth.ContainsKey(authParts[0])) {
                throw new Exception("Cannot contains map for auth: " + authParts[0]);
            }

            return string.Join(":", _mapAuth[authParts[0]], authParts[1], authParts[2]);
        }
    }
}
