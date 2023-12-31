using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Delivery;
using Talabat.Core.Specifications.OrderSpec;
using Talabat.Core.Specifications.Products;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService  _paymentService;

       

        public OrderService(
             IBasketRepository basketRepo,
                IUnitOfWork unitOfWork,
                IPaymentService paymentService


            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;

        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Baskets Repo
            var Basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItem>();

            if (Basket?.Items?.Count > 0) {
                foreach (var item in Basket.Items)
                {
                    var spec = new ProductWithTypeAndBrandSpecifications(item.Id);
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(spec);

                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
             
            // 3. Calculate SubTotal

            var subtotal = orderItems.Sum(i=>i.Price * i.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo
            var spec2 = new DeliveryMethodSpect(deliverMethodId);
          
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(spec2);
            // 5. Create Order

            var spec3 = new OrderWithPaymentIntentIdSpecification(Basket.PaymentIntentId);
            var existInOrder = await _unitOfWork.Repository<Order>().GetByIdAsync(spec3);
            if (existInOrder is not null)
            {
                 _unitOfWork.Repository<Order>().Delete(existInOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(Basket.Id);
            }
            var order = new Order(buyerEmail, orderItems, shippingAddress, deliveryMethod, subtotal,Basket.PaymentIntentId);
           
            await _unitOfWork.Repository<Order>().AddAsync(order);

            // 6. Save To Database [TODO]

            var result = await _unitOfWork.Complete();
           
            if (result <= 0) return null;

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var spec = new DeliveryMethodSpect();
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync(spec);
        }

        public async Task<Order> GetOrderById(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(spec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUser(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(buyerEmail);
           
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync(spec);

            return orders;
        }
    }
}
