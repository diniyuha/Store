using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem> GetOrderItem(int id);
        Task<int> AddOrderItem(OrderItem orderItem);
        Task UpdateOrderItem(OrderItem orderItem);
        Task DeleteOrderItem(int id);
    }
}