using Microsoft.AspNetCore.Mvc;

namespace AMST4.CAROUSEL.Models
{
    public class Product 
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }    
        public string Description { get; set; } 
        public string Price { get; set; }    
        public string ImageUrl { get; set; } 
        public Guid CategoryId { get; set; }    
        public virtual Category Category { get; set; }  
    }
}
