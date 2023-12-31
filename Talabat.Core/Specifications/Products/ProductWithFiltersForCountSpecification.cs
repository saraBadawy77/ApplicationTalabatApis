using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProdcutSpecParams SpecParams):
                   base(p=>
                    (string.IsNullOrEmpty(SpecParams.Search) || p.Name.ToLower().Contains(SpecParams.Search)) &&
                    (!SpecParams.BrandId.HasValue)||(p.ProductBrandId== SpecParams.BrandId) &&
                    (!SpecParams.TypeId.HasValue)||(p.ProductTypeId== SpecParams.TypeId))
        {

        }

    }
}
