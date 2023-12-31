﻿using AutoMapper;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAppApis.Dtos;

namespace TalabatAppApis.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public IConfiguration Configuration { get; }
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
                return $"{Configuration["ApiUrl"]}{source.ProductItemOrdered.PictureUrl}";
            return null;
        }
    }
}