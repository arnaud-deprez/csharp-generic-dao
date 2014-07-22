using Data.Access.Object.Test.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object.Test.Context
{
    public partial class MyDbContext : DbContext
    {
        #region IDbSet Declaration

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Manager> Managers { get; set; }
        public IDbSet<Skill> Skills { get; set; }

        #endregion

        #region Constructors

        public MyDbContext() : base()
        {
            Database.SetInitializer<MyDbContext>(new DropCreateDatabaseAlways<MyDbContext>());
        }

        public MyDbContext(IDatabaseInitializer<MyDbContext> initializer)
            : base()
        {
            Database.SetInitializer<MyDbContext>(initializer);
        }
        
        public MyDbContext(string nameOrConnectionString) : base(nameOrConnectionString) 
        {
            Database.SetInitializer<MyDbContext>(null);
        }

        public MyDbContext(string nameOrConnectionString, IDatabaseInitializer<MyDbContext> initializer) : base(nameOrConnectionString)
        {
            Database.SetInitializer<MyDbContext>(initializer);
        }

        public MyDbContext(DbConnection connection) : base(connection, true)
        {
            Database.SetInitializer<MyDbContext>(null);
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //add constraint for many to many project
            modelBuilder.Entity<Employee>()
                .HasMany<Skill>(e => e.Skills)
                .WithMany()
                .Map(x =>
                {
                    x.ToTable("EmployeeSkill");
                    x.MapLeftKey("EmployeeId");
                    x.MapRightKey("SkillId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
