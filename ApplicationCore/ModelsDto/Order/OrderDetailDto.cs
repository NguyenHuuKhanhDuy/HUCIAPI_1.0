﻿namespace ApplicationCore.ModelsDto.Order
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductNumber { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string ProductImage { get; set; } = null!;

        public int ProductPrice { get; set; }

        public int Discount { get; set; }

        public int SubTotal { get; set; }

        public int Quantity { get; set; }
    }
}
