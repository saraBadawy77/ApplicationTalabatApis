using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Delivery
{
    public class DeliveryMethodSpect:BaseSpecification<DeliveryMethod>
    {
        public DeliveryMethodSpect()
        {

        }

        public DeliveryMethodSpect(int id) : base(p => p.Id == id)
        {

        }
    }
}
