using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RuralAssets.WebApplication
{
    public class ApiCryptoMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiCryptoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICryptoService cryptoService)
        {
            if (!context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
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

            string requestContent;
            using (var reader = new StreamReader(context.Request.Body))
            {
                requestContent = await reader.ReadToEndAsync();
            }


            var requestJson = JsonConvert.SerializeObject(HandleJson(cryptoService.Decrypt, string.Empty,
                JsonConvert.DeserializeObject(requestContent)));
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

                var responseJson = JsonConvert.SerializeObject(HandleJson(cryptoService.Encrypt, string.Empty,
                    JsonConvert.DeserializeObject(responseJsonResult)));
                using (var responseStream = new MemoryStream(Encoding.UTF8.GetBytes(responseJson)))
                {
                    await responseStream.CopyToAsync(originalBodyStream);
                }
            }
        }

        private JObject HandleJson(Func<string, string> cryptoFunc, string key, object value)
        {
            var jObjectResult = new JObject();
            if (value is JObject jObject)
            {
                foreach (var o in jObject)
                {
                    if (o.Value.HasValues)
                    {
                        jObjectResult.Add(o.Key, HandleJson(cryptoFunc, o.Key, o.Value));
                    }
                    else
                    {
                        jObjectResult[o.Key] = cryptoFunc(o.Value.ToString());
                    }

                }
            }
            else if (value is JArray jArray)
            {
                var jArrayResult = new JArray();
                foreach (var o in jArray)
                {
                    jArrayResult.Add(HandleJson(cryptoFunc, string.Empty, o));
                }

                jObjectResult.Add(key, jArrayResult);
            }

            return jObjectResult;
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
}