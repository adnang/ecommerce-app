namespace ECommerceApp.Contracts.Response
{
    public class BasketResponse
    {
        public string Id { get; set; }
        public ProductItemRepsonse[] Items { get; set; }
    }
}