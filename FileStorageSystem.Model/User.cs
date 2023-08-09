using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileStorageSystem.Model
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }
        public string Passward { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool ActiveStatus { get; set; }
        [Display(Name = "Default Role")]
        public bool IsDefaultRole { get; set; }
    }
}
