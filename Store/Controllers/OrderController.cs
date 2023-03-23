using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Models;
using Store.Services;

namespace Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProviderService _providerService;

        public OrderController(IOrderService orderService, IProviderService providerService)
        {
            _orderService = orderService;
            _providerService = providerService;
        }

        public async Task<IActionResult> Index(OrderFilter filter)
        {
            if (!filter.OrderDateFrom.HasValue)
            {
                filter.OrderDateFrom = DateTime.Now.AddMonths(-1);
                filter.OrderDateTo = DateTime.Now;
            }

            var orders = await _orderService.GetOrders(filter);
            var providers = await _providerService.GetProviders();
            providers.Add(new Provider
            {
                Id = 0,
                Name = "-"
            });
            providers = providers.OrderBy(x => x.Id).ToList();
            SelectList providersList = new SelectList(providers, "Id", "Name", 0);

            ViewBag.Providers = providersList;
            return View(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            var providers = await _providerService.GetProviders();
            SelectList providersList = new SelectList(providers, "Id", "Name");
            ViewBag.Providers = providersList;

            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromForm] Order order)
        {
            await _orderService.CreateOrder(order);
            return RedirectToAction("Index", "Order");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromForm] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            await _orderService.UpdateOrder(id, order);

            return RedirectToAction("Index", "Order");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrder(id);

            return RedirectToAction("Index", "Order");
        }

        public async Task<IActionResult> AddOrder()
        {
            var providers = await _providerService.GetProviders();
            SelectList providersList = new SelectList(providers, "Id", "Name");
            ViewBag.Providers = providersList;
            return View("AddOrder");
        }

        public async Task<IActionResult> About()
        {
            return View("About");
        }
    }
}