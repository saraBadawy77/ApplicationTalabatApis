namespace TalabatAppApis.Extensions
{
    public  static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }







        public static WebApplication AddUseSwaggerMiddleware(this WebApplication application )
        {
           application.UseSwagger();
           application.UseSwaggerUI();


            return application;
        }
    }
}
