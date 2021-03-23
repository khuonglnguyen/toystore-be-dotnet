using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Decentralization")]
    public class Decentralization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int EmloyeeID { get; set; }
        public int RoleID { get; set; }
        public string Note { get; set; }
        [ForeignKey("EmloyeeID")]
        public virtual Emloyee Emloyee { set; get; }
        [ForeignKey("RoleID")]
        public virtual Role Role { set; get; }
    }
}