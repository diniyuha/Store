using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Store.Models;
using Store.Services;

namespace Store.Controllers
{
    [Route("[controller]/[action]")]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _orderItemService.GetOrderItem(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            await _orderItemService.AddOrderItem(orderItem);
            return RedirectToAction("GetOrderById", "Order", new {id = orderItem.OrderId});
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromForm] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return BadRequest();
            }

            await _orderItemService.UpdateOrderItem(orderItem);
            return RedirectToAction("GetOrderById", "Order", new {id = orderItem.OrderId});
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderItemService.GetOrderItem(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            await _orderItemService.DeleteOrderItem(id);
            return RedirectToAction("GetOrderById", "Order", new {id = orderItem.OrderId});
        }

        [HttpGet("AddView/{orderId}")]
        public IActionResult AddOrderItem(int orderId)
        {
            return View(orderId);
        }
    }
}