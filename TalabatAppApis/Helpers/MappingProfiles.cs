using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAppApis.Dtos;

namespace TalabatAppApis.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<AddressDto,Talabat.Core.Entities.Order_Aggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
               //  .ForMember(d => d.PaymentIntentId, o => o.MapFrom(s => !string.IsNullOrEmpty(s.PaymentIntentId) ? s.PaymentIntentId : null))
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, O => O.MapFrom(S => S.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.ProductItemOrdered.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.ProductItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.ProductItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());


        }


    }
}
