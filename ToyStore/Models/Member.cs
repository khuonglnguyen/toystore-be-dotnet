using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int MemberCategoryID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Capcha { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("MemberCategoryID")]
        public virtual MemberCategory MemberCategory { set; get; }
    }
}