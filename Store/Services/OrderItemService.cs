using System;
using System.Threading.Tasks;
using AutoMapper;
using Store.Models;
using Store_entities;
using Store_entities.Entities;

namespace Store.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly StoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderItemService(StoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<OrderItem> GetOrderItem(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                throw new ArgumentException("Not found");
            }

            return _mapper.Map<OrderItem>(orderItem);
        }

        public async Task<int> AddOrderItem(OrderItem orderItem)
        {
            var order = await _dbContext.Orders.FindAsync(orderItem.OrderId);
            if (order != null && orderItem.Name == order.Number)
            {
               throw new ArgumentException("Name of item should not equal number of order");
            }

            var orderItemEntity = _mapper.Map<OrderItemEntity>(orderItem);
            _dbContext.OrderItems.Add(orderItemEntity);
            await _dbContext.SaveChangesAsync();
            return orderItemEntity.Id;
        }

        public async Task UpdateOrderItem(OrderItem orderItem)
        {
            var orderItemEntity = await _dbContext.OrderItems.FindAsync(orderItem.Id);
            if (orderItemEntity == null)
            {
                throw new ArgumentException("Not found");
            }

            _mapper.Map(orderItem, orderItemEntity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderItem(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                throw new ArgumentException("Not found");
            }

            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}