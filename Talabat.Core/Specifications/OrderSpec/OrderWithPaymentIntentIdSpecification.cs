﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentIdSpecification : BaseSpecification<Order>
    {

        public OrderWithPaymentIntentIdSpecification(string paymentId) : base(O => O.PaymentIntentId == paymentId)
        {

        }
    }
}
