using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store_entities.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }

        public int ProviderId { get; set; }
        public ProviderEntity Provider { get; set; }

        public ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}