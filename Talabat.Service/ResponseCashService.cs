using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase database;
        public ResponseCashService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task CashResponseAsync(string CashKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;
            var options = new JsonSerializerOptions() {PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
            var serializedresponse = JsonSerializer.Serialize(Response, options);
            await database.StringSetAsync(CashKey, serializedresponse, TimeToLive);
        }

        public async Task<string> GetCashedResponse(string CashKey)
        {
            var CashedResponse = await database.StringGetAsync(CashKey);
            if (CashedResponse.IsNullOrEmpty)
                return null;
            return CashedResponse;
        }
    }
}
