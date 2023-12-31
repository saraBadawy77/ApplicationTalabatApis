using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace TalabatAppApis.Helpers
{

    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _TimeToLiveInSeconds;
        public CashedAttribute(int TimeToLiveInSeconds)
        {
            _TimeToLiveInSeconds = TimeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CashService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();
            var CasheKey = GenerateCashKeyFromRequestBody(context.HttpContext.Request);
            var CashedResponse = await CashService.GetCashedResponse(CasheKey);
            if (!string.IsNullOrEmpty(CashedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = CashedResponse,
                    ContentType = "Application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var ExcutedContext = await next();
            if (ExcutedContext.Result is OkObjectResult objectResult)
                await CashService.CashResponseAsync(CasheKey, objectResult.Value, TimeSpan.FromSeconds(_TimeToLiveInSeconds));
        }
        private string GenerateCashKeyFromRequestBody(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append($"{request.Path}");

            foreach (var (Key, Value) in request.Query.OrderBy(x => x.Key))
                KeyBuilder.Append($"{Key}-{Value}");

            return KeyBuilder.ToString();
        }
    }
}
