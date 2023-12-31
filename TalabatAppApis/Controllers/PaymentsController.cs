using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using TalabatAppApis.Dtos;
using TalabatAppApis.Errors;

namespace TalabatAppApis.Controllers
{
    [Authorize]
    public class PaymentsController :BaseAPIController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _webhook = "whsec_74de6a645bde4158438d88e6e83b9f763b63f45992d304b8f3def4daf56e5d2d";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("CreateOrUpdatePaymentIntent")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400, "A Problem With Your Basket"));

            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var Json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var StripeEvent = EventUtility.ConstructEvent(Json,
                Request.Headers["Stripe-Signature"], _webhook);
            var paymentIntent = (PaymentIntent)StripeEvent.Data.Object;
            Order order;
            switch (StripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is succeeded", paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment is failed", paymentIntent.Id);
                    break;
            }
            return Ok();
        }


    }
}
