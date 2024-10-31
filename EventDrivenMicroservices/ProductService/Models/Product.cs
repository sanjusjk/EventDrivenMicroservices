﻿namespace ProductService.Models;

public class Product
{
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
}