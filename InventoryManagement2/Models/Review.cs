using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
