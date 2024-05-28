﻿namespace AMST4.CAROUSEL.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual  List<Product> Products { get; set;}
    }
}
