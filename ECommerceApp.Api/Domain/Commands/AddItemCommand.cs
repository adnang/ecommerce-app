namespace ECommerceApp.Api.Domain.Commands
{
    public class AddItemCommand
    {
        public string Sku { get; set; }
        public string Description { get; set; }
    }
}