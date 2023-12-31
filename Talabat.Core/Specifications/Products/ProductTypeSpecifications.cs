using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products
{
    public class ProductTypeSpecifications : BaseSpecification<ProductType>
    {
        public ProductTypeSpecifications()
        {

        }

        public ProductTypeSpecifications(int id) : base(p => p.Id == id)
        {

        }
    }
}
