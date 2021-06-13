using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommerceApplication.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [MaxLength(100)]
        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [NotMapped]
        public string ProductImage { get; set; }

        [Display(Name = "Upload File")]
        public byte[] ProductImageData { get; set; }

        [MaxLength(250)]
        public string ProductDescription { get; set; }

        [Required]
        public string Category { get; set; }

        public string Owner { get; set; }
        public DateTime CreatedDttm { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDttm { get; set; }
    }
}
