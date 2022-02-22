using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
 
        [Required(ErrorMessage = "is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "is required")]
        public float Price { get; set; }
 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<ProductInCategory> Categories { get; set; }
    }
}