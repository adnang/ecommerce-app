namespace ECommerceApp.Api.Domain
{
    public class ProductItem
    {
        public string Sku { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; } = 1;
    }
}