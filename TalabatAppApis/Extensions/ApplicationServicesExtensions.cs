using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Service;
using TalabatAppApis.Errors;
using TalabatAppApis.Helpers;

namespace TalabatAppApis.Extensions
{
    public  static class ApplicationServicesExtensions
    {
        public static IServiceCollection  AddApplicationServices(this IServiceCollection services)
        {

            services.AddSingleton<IResponseCashService,ResponseCashService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(
                options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                        .SelectMany(p => p.Value.Errors)
                        .Select(p => p.ErrorMessage)
                        .ToArray();


                        var validationErrorResponse = new ApiValidationErrorResponse()
                        {
                            Errors = errors
                        };

                        return new BadRequestObjectResult(validationErrorResponse);

                    };

                });

            return services;
        }

    }
}
