using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Shop.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [ForeignKey("Category")] 
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        
        public string ImageUrl { get; set; } = string.Empty;
    }
}