using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications.Products
{
    public class ProductWithTypeAndBrandSpecifications : BaseSpecification<Product>
    {



        public ProductWithTypeAndBrandSpecifications(ProdcutSpecParams SpecParams) :
            base(p=>
            (string.IsNullOrEmpty(SpecParams.Search) || p.Name.ToLower().Contains(SpecParams.Search)) &&
            (!SpecParams.BrandId.HasValue)||(p.ProductBrandId== SpecParams.BrandId) &&
            (!SpecParams.TypeId.HasValue)||(p.ProductTypeId== SpecParams.TypeId))
            
        {

            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);



            ApplyPagination(SpecParams.PageSize * (SpecParams.PageIndex - 1), SpecParams.PageSize);


            if (!string.IsNullOrEmpty(SpecParams.Sort))
            {
                switch (SpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }


        }

        public ProductWithTypeAndBrandSpecifications(int id ):base(p=>p.Id==id)
        {

            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }

    }
}
