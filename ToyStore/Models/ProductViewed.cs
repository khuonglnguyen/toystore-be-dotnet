using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("ProductViewed")]
    public class ProductViewed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Member { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }
    }
}