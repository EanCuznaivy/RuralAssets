using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using AElf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Caching;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RuralAssets.WebApplication
{
    public class ApiMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICryptoService cryptoService,
            IDistributedCache<NonceCache> nonceCache, IOptionsSnapshot<ApiAuthorizeOptions> apiAuthorizeOptions,
            IOptionsSnapshot<ModuleConfigOptions> moduleOptions)
        {
            context.Request.EnableBuffering();

            string requestBody;
            using (var stream = new StreamReader(context.Request.Body))
            {
                requestBody = await stream.ReadToEndAsync();
            }

            if (context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase) &&
                moduleOptions.Value.EnableAuthorization)
            {
                var request = context.Request;
                request.Headers.TryGetValue("appid", out var appId);
                request.Headers.TryGetValue("nonce", out var nonce);
                request.Headers.TryGetValue("timestamp", out var timestamp);
                request.Headers.TryGetValue("signature", out var signature);

                var verifyResult = VerifySignature(appId, Convert.ToInt64(timestamp), nonce, signature, request.Query,
                    JsonConvert.DeserializeObject(requestBody), nonceCache,
                    apiAuthorizeOptions.Value);
                if (verifyResult != string.Empty)
                {
                    var result = new ResponseDto
                    {
                        Code = cryptoService.Encrypt(MessageHelper.GetCode(MessageHelper.Message.AuthorizeFailed)),
                        Msg = cryptoService.Encrypt(MessageHelper.GetMessage(MessageHelper.Message.AuthorizeFailed)),
                        //Description = verifyResult
                    };
                    context.Response.ContentType = "application/json; charset=utf-8; v=1.0";
                    var options = new JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result, options), Encoding.UTF8);
                    return;
                }
            }

            var requestPath = context.Request.Path.Value;
            if (!requestPath.StartsWith("/api", StringComparison.OrdinalIgnoreCase) ||
                requestPath.EndsWith("upload", StringComparison.OrdinalIgnoreCase) ||
                !moduleOptions.Value.EnableCrypto)
            {
                await _next(context);
                return;
            }

            var dic = new Dictionary<string, StringValues>();
            foreach (var query in context.Request.Query)
            {
                dic.Add(query.Key, new StringValues(cryptoService.Decrypt(query.Value)));
            }

            context.Request.Query = new QueryCollection(dic);

            var requestJson = JsonConvert.SerializeObject(HandleJson(cryptoService.Decrypt,
                JsonConvert.DeserializeObject(requestBody)));
            using (var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestJson)))
            {
                context.Request.Body = requestStream;

                string responseJsonResult;
                var originalBodyStream = context.Response.Body;

                using (var ms = new MemoryStream())
                {
                    context.Response.Body = ms;
                    await _next(context);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var sr = new StreamReader(ms))
                    {
                        responseJsonResult = sr.ReadToEnd();
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                }

                var responseJson = JsonConvert.SerializeObject(HandleJson(cryptoService.Encrypt,
                    JsonConvert.DeserializeObject(responseJsonResult)));
                using (var responseStream = new MemoryStream(Encoding.UTF8.GetBytes(responseJson)))
                {
                    await responseStream.CopyToAsync(originalBodyStream);
                }
            }
        }

        private string VerifySignature(string appId, long timestamp, string nonce, string signature,
            IQueryCollection query, object body, IDistributedCache<NonceCache> nonceCache,
            ApiAuthorizeOptions apiAuthorizeOptions)
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            var utcNow = DateTimeOffset.UtcNow;
            if (time > utcNow || time < utcNow.AddMinutes(-10))
            {
                return "时间戳超时（10分钟）";
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
                return "nonce已存在";
            }

            var message = $"appid={appId}nonce={nonce}timestamp={timestamp}";

            if (query.Count > 0)
            {
                var queryString = query.OrderBy(q => q.Key).Aggregate(string.Empty,
                    (current, value) => current + (value.Key + "=" + value.Value));
                message += queryString;
            }

            var sortedString = GetSortedString(body);
            message += sortedString;

            var appsecret = apiAuthorizeOptions.AppAccount[appId];
            byte[] computedSignature;
            using (var hmac = new HMACSHA256(appsecret.GetBytes(Encoding.Default)))
            {
                computedSignature = hmac.ComputeHash(message.GetBytes(Encoding.Default));
            }

            var correctSignature = computedSignature.ToHex();
            if (correctSignature != signature)
            {
                return $"签名验证失败，应为：{correctSignature}";
            }

            return string.Empty;
        }

        private string GetSortedString(object obj)
        {
            var sortedString = string.Empty;
            if (obj is JObject)
            {
                var sortedDictionary =
                    JsonConvert.DeserializeObject<SortedDictionary<string, dynamic>>(obj.ToString());

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
                return obj.ToString().Trim();
            }

            return sortedString;
        }


        private JToken HandleJson(Func<string, string> cryptoFunc, object obj)
        {
            if (obj is JObject jObject)
            {
                var jObjectResult = new JObject();
                foreach (var o in jObject)
                {
                    if (o.Value.HasValues)
                    {
                        jObjectResult[o.Key] = HandleJson(cryptoFunc, o.Value);
                    }
                    else
                    {
                        jObjectResult[o.Key] = cryptoFunc(o.Value.ToString());
                    }
                }

                return jObjectResult;
            }

            if (obj is JArray jArray)
            {
                var jArrayResult = new JArray();
                foreach (var o in jArray)
                {
                    jArrayResult.Add(HandleJson(cryptoFunc, o));
                }

                return jArrayResult;
            }

            throw new InvalidOperationException();
        }
    }

    public class NonceCache
    {
        public string Nonce { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}