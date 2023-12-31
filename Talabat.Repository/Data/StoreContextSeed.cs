using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {

            if (!context.ProductBrands.Any())
            {

                var BrandsData = File.ReadAllText("C:/Users/sara/Desktop/Innovascop/TalabatAppApis/Talabat.Repository/Data/DataSeed/brands.json");

                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var Brand in Brands)
                        await context.Set<ProductBrand>().AddAsync(Brand);
                    await context.SaveChangesAsync();
                }


            }

            if (!context.ProductTypes.Any())
            {

                var TypesData = File.ReadAllText("C:/Users/sara/Desktop/Innovascop/TalabatAppApis/Talabat.Repository/Data/DataSeed/types.json");

                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types is not null && Types.Count > 0)
                {
                    foreach (var Type in Types)
                        await context.Set<ProductType>().AddAsync(Type);
                    await context.SaveChangesAsync();
                }


            }


            if (!context.Products.Any())
            {

                var ProductsData = File.ReadAllText("C:/Users/sara/Desktop/Innovascop/TalabatAppApis/Talabat.Repository/Data/DataSeed/products.json");

                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products is not null && Products.Count > 0)
                {
                    foreach (var Product in Products)
                        await context.Set<Product>().AddAsync(Product);
                   
                    
                    await context.SaveChangesAsync();
                }


            }

            if (!context.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("C:/Users/sara/Desktop/Innovascop/TalabatAppApis/Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods is not null && DeliveryMethods.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                        await context.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    await context.SaveChangesAsync();
                }
            }



        }


    }
}
