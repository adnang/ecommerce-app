namespace ECommerceApp.Api.Domain.Commands
{
    public class UpdateItemCommand
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
    }
}