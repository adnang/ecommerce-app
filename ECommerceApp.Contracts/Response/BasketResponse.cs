namespace ECommerceApp.Contracts.Response
{
    public class BasketResponse
    {
        public string Id { get; set; }
        public ProductItemResponse[] Items { get; set; }
    }
}