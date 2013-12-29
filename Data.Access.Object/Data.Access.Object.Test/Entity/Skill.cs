using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object.Test.Entity
{
    public partial class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        #region Foreign Keys

        public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        public ICollection<Employee> Employees { get; set; }
        #endregion
    }
}
