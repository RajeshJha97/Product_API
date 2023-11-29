using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }       
        
        [Required]
        public string Name { get; set; }       
        
        [Required]
        public string Description { get; set; }
        
        [Required]        
        public string Category { get; set; }
        
        [Required]
        public double Price { get; set; }


        [NotMapped]
        public int Calculations { get; set; }
    }
}
