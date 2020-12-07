using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace RuralAssets.WebApplication
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var dic = new Dictionary<string, StringValues>();
            foreach (var query in context.Request.Query)
            {
                // Decrypt
                var newValue = query.Value.ToString();
                dic.Add(query.Key, new StringValues(newValue));
            }

            context.Request.Query = new QueryCollection(dic);

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
                    await ms.CopyToAsync(originalBodyStream);
                    
                    // Encrypt
                    var responseJson = responseJsonResult + "new value";
                    await context.Response.WriteAsync(responseJson, Encoding.UTF8);
                }
            }
        }
    }
}