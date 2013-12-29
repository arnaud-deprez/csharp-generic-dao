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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Firstname { get; set; }
        [EmailAddress(ErrorMessage = "The email address is not correct")]
        public string Email { get; set; }
        [RegularExpression("+\\d{2,}", ErrorMessage = "The Phone Number is not valid !")]
        public string PhoneNumber { get; set; }

        #region Foreign Keys

        [ForeignKey("Manager")]
        public Guid ManagerId { get; set; }
        public Manager Manager { get; set; }
        [ForeignKey("Company")]
        public Nullable<long> CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        public ICollection<Skill> Skills { get; set; }

        #endregion
    }
}
