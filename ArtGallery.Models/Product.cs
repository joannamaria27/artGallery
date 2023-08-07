using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }
        [Required]
        [DisplayName("Number of pieces created")]
        [Range(1, 1000000, ErrorMessage = "Stock Quantity must be between 1-1000000")]
        public int StockQuantity { get; set; }
        [Required]
        [DisplayName("Created date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
