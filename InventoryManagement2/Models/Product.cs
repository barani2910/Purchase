using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime ManufacturingDate { get; set; }

        public string Description { get; set; }

        [Required]
        public int Stock { get; set; }

        public string ImagePath { get; set; }  

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
