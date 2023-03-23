using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Store.MapProfiles;
using Store.Models;
using Store.Services;
using Store_entities;

namespace store_tests
{
    public class OrderItemServiceTests
    {
        static DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseInMemoryDatabase(databaseName: "Store")
            .Options;

        private StoreDbContext _appDbContext;
        private IOrderService _orderService;
        private IOrderItemService _orderItemService;
        private IProviderService _providerService;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapProfiles>());
            _mapper = new Mapper(config);
            _appDbContext = new StoreDbContext(options);
            _appDbContext.Database.EnsureDeleted();
            _orderService = new OrderService(_appDbContext, _mapper);
            _orderItemService = new OrderItemService(_appDbContext, _mapper);
            _providerService = new ProviderService(_appDbContext, _mapper);
        }

        [Test]
        public async Task GetItemOrderById_ShouldReturnItemById()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var itemOrderId = await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            var result = await _orderItemService.GetOrderItem(itemOrderId);
            Assert.That(result.Id, Is.EqualTo(itemOrderId));
        }

        [Test]
        public async Task AddOrderItem_ShouldCreateNewItem()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var itemOrderId = await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            var result = await _orderItemService.GetOrderItem(itemOrderId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(itemOrderId));
        }

        [Test]
        public async Task AddOrderItemWithNameEqualsOrdersNumber_ShouldReturnException()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _orderItemService.AddOrderItem(new OrderItem
                {
                    OrderId = orderId, Name = "111", Quantity = 15.800M, Unit = "kg"
                });
            });
            if (ex != null) Assert.That(ex.Message, Is.EqualTo("Name of item should not equal number of order"));
        }

        [Test]
        public async Task UpdateOrderItem_ShouldUpdateItem()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var itemOrderId = await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            var orderItem = await _orderItemService.GetOrderItem(itemOrderId);

            orderItem.Name = "Bread";

            await _orderItemService.UpdateOrderItem(orderItem);

            var result = await _orderItemService.GetOrderItem(itemOrderId);

            Assert.That(result.Name, Is.EqualTo("Bread"));
        }

        [Test]
        public async Task DeleteOrderItem_ShouldDeleteOrderItemById()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var itemOrderId = await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            var result = await _orderItemService.GetOrderItem(itemOrderId);
            Assert.That(result, Is.Not.Null);

            await _orderItemService.DeleteOrderItem(itemOrderId);

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                result = await _orderItemService.GetOrderItem(itemOrderId);
            });
            if (ex != null) Assert.That(ex.Message, Is.EqualTo("Not found"));
        }

        [Test]
        public async Task DeleteOrderItem_ShouldReturnExceptionOrderItemNotFound()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var itemOrderId = await _orderItemService.AddOrderItem(new OrderItem
            {
                OrderId = orderId, Name = "Cheese", Quantity = 15.800M, Unit = "kg"
            });

            var result = await _orderItemService.GetOrderItem(itemOrderId);
            Assert.That(result, Is.Not.Null);

            itemOrderId = 2;

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _orderItemService.DeleteOrderItem(itemOrderId);
            });
            if (ex != null) Assert.That(ex.Message, Is.EqualTo("Not found"));
        }
    }
}