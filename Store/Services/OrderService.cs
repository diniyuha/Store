using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store_entities;
using Store_entities.Entities;

namespace Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(StoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Order>> GetOrders(OrderFilter filter = null)
        {
            var query = _dbContext.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.Provider)
                .AsNoTracking()
                .AsQueryable();

            if (filter != null)
            {
                if (filter.ProviderId.HasValue && filter.ProviderId.Value > 0)
                {
                    query = query.Where(o => o.ProviderId == filter.ProviderId);
                }

                if (!string.IsNullOrEmpty(filter.Number))
                {
                    query = query.Where(o => o.Number.Contains(filter.Number));
                }

                if (!string.IsNullOrEmpty(filter.ItemName))
                {
                    query = query.Where(o => o.OrderItems.Any(x => x.Name.Contains(filter.ItemName)));
                }

                if (!string.IsNullOrEmpty(filter.Unit))
                {
                    query = query.Where(o => o.OrderItems.Any(x => x.Unit == filter.Unit));
                }

                if (filter.OrderDateFrom.HasValue)
                {
                    query = query.Where(o => o.Date >= filter.OrderDateFrom.Value);
                }

                if (filter.OrderDateTo.HasValue)
                {
                    query = query.Where(o => o.Date <= filter.OrderDateTo.Value);
                }
            }

            var orders = await query.ToListAsync();
            return _mapper.Map<List<Order>>(orders);
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _dbContext.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                throw new ArgumentException("Not found");
            }

            return _mapper.Map<Order>(order);
        }

        public async Task<int> CreateOrder(Order order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            _dbContext.Orders.Add(orderEntity);
            await _dbContext.SaveChangesAsync();
            return orderEntity.Id;
        }

        public async Task UpdateOrder(int id, Order order)
        {
            var orderEntity = await _dbContext.Orders.FindAsync(id);
            if (orderEntity == null)
            {
                throw new ArgumentException("Not found");
            }

            _mapper.Map(order, orderEntity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new ArgumentException("Not found");
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}