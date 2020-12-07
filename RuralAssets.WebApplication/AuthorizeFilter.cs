using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Threading;
using Volo.Abp.Caching;
using Volo.Abp.Json;


namespace RuralAssets.WebApplication
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        private readonly IDistributedCache<NonceCache> _nonceCache;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ApiAuthorizeOptions _apiAuthorizeOptions;

        public AuthorizeFilter(IDistributedCache<NonceCache> nonceCache, IJsonSerializer jsonSerializer,
            IOptionsSnapshot<ApiAuthorizeOptions> apiAuthorizeOptions)
        {
            _nonceCache = nonceCache;
            _jsonSerializer = jsonSerializer;
            _apiAuthorizeOptions = apiAuthorizeOptions.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            request.Headers.TryGetValue("appid", out var appId);
            request.Headers.TryGetValue("nonce", out var nonce);
            request.Headers.TryGetValue("timestamp", out var timestamp);
            request.Headers.TryGetValue("signature", out var signature);

            string requestBody;
            using (var stream = new StreamReader(request.Body))
            {
                stream.BaseStream.Position = 0;
                requestBody = AsyncHelper.RunSync(stream.ReadToEndAsync);
            }

            var verifyResult = VerifySignature(appId, Convert.ToInt64(timestamp), nonce, signature, request.Query,
                requestBody);
            if (!verifyResult)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool VerifySignature(string appId, long timestamp, string nonce, string signature,
            IQueryCollection query, string body)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            var utcNow = DateTimeOffset.UtcNow;
            if (time > utcNow || time < utcNow.AddMinutes(-10))
            {
                return false;
            }

            var nonceExist = true;
            _nonceCache.GetOrAdd(nonce, () =>
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

            var appsecret = _apiAuthorizeOptions.AppAccount[appId];
            byte[] computedSignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appsecret)))
            {
                computedSignature = hmac.ComputeHash(messageBytes);
            }

            if (Convert.ToBase64String(computedSignature) != signature)
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
                sortedString = _jsonSerializer.Serialize(jArray);
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