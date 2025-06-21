namespace SharedKernel.Contracts.Events
{
    public class OrderPaidEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid ShippingCompanyId { get; set; }
        public List<ProductItem> Products { get; set; }

        public class ProductItem
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}