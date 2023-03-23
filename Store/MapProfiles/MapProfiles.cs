using AutoMapper;
using Store.Models;
using Store_entities.Entities;

namespace Store.MapProfiles
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<OrderEntity, Order>()
                .ForMember(d => d.ItemsCount, o => o.MapFrom(s => s.OrderItems.Count))
                .ReverseMap();

            CreateMap<OrderItemEntity, OrderItem>();

            CreateMap<OrderItem, OrderItemEntity>()
                .ForMember(d => d.Order, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<ProviderEntity, Provider>()
                .ReverseMap();
        }
    }
}