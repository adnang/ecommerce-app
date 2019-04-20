using System;

namespace ECommerceApp.Client.Configuration
{
    public class BasketApiConfiguration
    {
        public Uri BaseAddress { get; set; }
        public TimeSpan ConnectionTimeout { get; set; }
    }
}