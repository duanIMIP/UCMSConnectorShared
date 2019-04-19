using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using UCMS.Core;
using UCMS.Model;
using UCMS.RestClient;

namespace UCMS.ImportController
{
    internal static class TokenCache
    {
        public static Token GetToken(string clientId)
        {
            var cacheKey = $"Auth-{clientId}";
            if (MemoryCache.Default.Contains(cacheKey))
            {
                return MemoryCache.Default.Get(cacheKey) as Token;
            }

            var tk = TokenHelper.GetAccessToken(ConnectedApp.ActivityWorker, clientId);

            var token = new Token
            {
                access_token = tk["access_token"],
                refresh_token = tk["refresh_token"]
            };

            MemoryCache.Default.Add(cacheKey, token, DateTimeOffset.Now.AddHours(1));
            return token;
        }
    }
}
