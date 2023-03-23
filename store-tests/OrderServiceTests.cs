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
    [TestFixture]
    public class OrderServiceTests
    {
        static DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseInMemoryDatabase(databaseName: "Store")
            .Options;

        private StoreDbContext _appDbContext;
        private IOrderService _orderService;
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
            _providerService = new ProviderService(_appDbContext, _mapper);
        }

        [Test]
        public async Task GetOrders_ShouldReturnAllOrder()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });
            await _orderService.CreateOrder(new Order
            {
                Number = "222", Date = DateTime.Now, ProviderId = providerId
            });

            var result = await _orderService.GetOrders(new OrderFilter());
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task CreateAndGetOrder_ShouldReturnOrderById()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });
            var result = await _orderService.GetOrderById(orderId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(orderId));
        }

        [Test]
        public async Task Update_ShouldUpdateOrderById()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });
            var order = await _orderService.GetOrderById(orderId);

            order.Number = "222";
            await _orderService.UpdateOrder(orderId, order);

            var result = await _orderService.GetOrderById(orderId);

            Assert.That(result.Number, Is.EqualTo("222"));
        }

        [Test]
        public async Task Delete_ShouldDeleteOrderById()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var result = await _orderService.GetOrderById(orderId);
            Assert.That(result, Is.Not.Null);

            await _orderService.DeleteOrder(orderId);

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                result = await _orderService.GetOrderById(orderId);
            });
            if (ex != null) Assert.That(ex.Message, Is.EqualTo("Not found"));
        }

        [Test]
        public async Task Delete_ShouldReturnExceptionOrderNotFound()
        {
            var providerId = await _providerService.CreateProvider("providerName");
            var orderId = await _orderService.CreateOrder(new Order
            {
                Number = "111", Date = DateTime.Now, ProviderId = providerId
            });

            var result = await _orderService.GetOrderById(orderId);
            Assert.That(result, Is.Not.Null);

            orderId = 2;

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _orderService.DeleteOrder(orderId);
            });
            if (ex != null) Assert.That(ex.Message, Is.EqualTo("Not found"));
        }
    }
}