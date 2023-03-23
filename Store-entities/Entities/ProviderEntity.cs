using System.Collections.Generic;

namespace Store_entities.Entities
{
    public class ProviderEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}