using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object.Test.Entity
{
    public partial class Manager : Employee
    {
        public Manager()
        {
            Employees = new HashSet<Employee>();
        }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
