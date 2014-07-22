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
        public Skill()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        #region Foreign Keys

        public virtual ICollection<Employee> Employees { get; set; }

        #endregion
    }
}
