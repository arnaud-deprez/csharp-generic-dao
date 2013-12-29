using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object.Test.Entity
{
    public partial class EmployeeSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        #region Foreign Keys

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [ForeignKey("Skill")]
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; }

        #endregion
    }
}
