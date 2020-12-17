using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AElf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Caching;

namespace RuralAssets.WebApplication
{
    public class ApiAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICryptoService cryptoService,
            IDistributedCache<NonceCache> nonceCache, IOptionsSnapshot<ApiAuthorizeOptions> apiAuthorizeOptions,
            IOptionsSnapshot<ModuleConfigOptions> moduleOptions)
        {
            if (context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase) &&
                moduleOptions.Value.EnableAuthorization)
            {
                var request = context.Request;
                request.Headers.TryGetValue("appid", out var appId);
                request.Headers.TryGetValue("nonce", out var nonce);
                request.Headers.TryGetValue("timestamp", out var timestamp);
                request.Headers.TryGetValue("signature", out var signature);

                string requestBody;
                using (var stream = new StreamReader(request.Body))
                {
                    requestBody = await stream.ReadToEndAsync();
                }

                var verifyResult = VerifySignature(appId, Convert.ToInt64(timestamp), nonce, signature, request.Query,
                    requestBody, nonceCache, apiAuthorizeOptions.Value);
                if (!verifyResult)
                {
                    var result = new ResponseDto
                    {
                        Code = cryptoService.Encrypt(MessageHelper.GetCode(MessageHelper.Message.AuthorizeFailed)),
                        Msg = cryptoService.Encrypt(MessageHelper.GetMessage(MessageHelper.Message.AuthorizeFailed))
                    };
                    context.Response.ContentType = "application/json; charset=utf-8; v=1.0";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
                    return;
                }
            }

            await _next(context);
        }
        
        private bool VerifySignature(string appId, long timestamp, string nonce, string signature,
            IQueryCollection query, string body, IDistributedCache<NonceCache> nonceCache,ApiAuthorizeOptions apiAuthorizeOptions)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            var utcNow = DateTimeOffset.UtcNow;
            if (time > utcNow || time < utcNow.AddMinutes(-10))
            {
                return false;
            }

            var nonceExist = true;
            nonceCache.GetOrAdd(nonce, () =>
            {
                nonceExist = false;
                return new NonceCache {Nonce = nonce, Timestamp = time};
            }, () =>
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = time.AddMinutes(10)
                });

            if (nonceExist)
            {
                return false;
            }

            var message = $"appid={appId}nonce={nonce}timestamp={timestamp}";

            if (query.Count > 0)
            {
                var queryString = query.OrderBy(q => q.Key).Aggregate(string.Empty,
                    (current, value) => current + (value.Key + "=" + value.Value));
                message += queryString;
            }

            if (!string.IsNullOrWhiteSpace(body))
            {
                var sortedString = GetSortedString(body);
                message += sortedString;
            }

            var messageBytes = Encoding.UTF8.GetBytes(message);

            var appsecret = apiAuthorizeOptions.AppAccount[appId];
            byte[] computedSignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appsecret)))
            {
                computedSignature = hmac.ComputeHash(messageBytes);
            }

            if (computedSignature.ToHex() != signature)
            {
                return false;
            }

            return true;
        }

        private string GetSortedString(object obj)
        {
            var sortedString = string.Empty;
            if (obj is JObject)
            {
                var sortedDictionary =
                    JsonConvert.DeserializeObject<SortedDictionary<string, dynamic>>(JsonConvert.SerializeObject(obj));

                sortedString = sortedDictionary.OrderBy(d => d.Key).Aggregate(sortedString,
                    (current, param) => (string) (current + (param.Key + "=" + GetSortedString(param.Value))));
            }
            else if (obj is JArray jArray)
            {
                foreach (var o in jArray)
                {
                    sortedString += GetSortedString(o);
                }
            }
            else
            {
                return obj.ToString();
            }

            return sortedString;
        }
    }
    
    public class NonceCache
    {
        public string Nonce { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}