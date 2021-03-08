using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int ProductID { set; get; }
        public int MemberID { set; get; }
        public int Star { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }
        [ForeignKey("MemberID")]
        public virtual Member Member { set; get; }
    }
}