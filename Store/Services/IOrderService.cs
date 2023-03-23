using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders(OrderFilter filter = null);
        Task<Order> GetOrderById(int id);
        Task<int> CreateOrder(Order order);
        Task DeleteOrder(int id);
        Task UpdateOrder(int id,  Order order);
    }
}
