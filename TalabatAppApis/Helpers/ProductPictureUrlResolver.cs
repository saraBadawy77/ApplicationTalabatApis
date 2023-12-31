using AutoMapper;
using Talabat.Core.Entities;
using TalabatAppApis.Dtos;

namespace TalabatAppApis.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, String>
    {
        private readonly IConfiguration _Configuration;
        public ProductPictureUrlResolver(IConfiguration configuration )
        {
            _Configuration = configuration;
        }

       

        string IValueResolver<Product, ProductToReturnDto, string>.Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))

                return $"{_Configuration["APIBaseUrl"]}{source.PictureUrl}";
           
            return $"{string.Empty}";
        }
    }
}
