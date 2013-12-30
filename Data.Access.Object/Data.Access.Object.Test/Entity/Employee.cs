using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object.Test.Entity
{
    public partial class Employee
    {
        public Employee()
        {
            Skills = new HashSet<Skill>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Firstname { get; set; }
        [EmailAddress(ErrorMessage = "The email address is not correct")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "The Phone Number is not valid !")]
        public string PhoneNumber { get; set; }

        #region Foreign Keys

        public long? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Manager Boss { get; set; }
        public Nullable<long> CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

        #endregion
    }
}
