﻿namespace Vendor.Models
{
    public class Cart
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string productPic { get; set; }
        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }

    }
}