using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [MaxLength(256)]
        public string Name { set; get; }

        public string Address { set; get; }

        public string Phone { set; get; }

        public string Email { set; get; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public virtual IEnumerable<Product> Products { set; get; }
    }
}