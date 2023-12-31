using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products
{
    public class ProductBrandSpecifications : BaseSpecification<ProductBrand>
    {
        public ProductBrandSpecifications()
        {

        }

        public ProductBrandSpecifications(int id) : base(p => p.Id == id)
        {

        }
    }
}
