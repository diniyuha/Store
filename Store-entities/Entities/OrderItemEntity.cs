namespace Store_entities.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}