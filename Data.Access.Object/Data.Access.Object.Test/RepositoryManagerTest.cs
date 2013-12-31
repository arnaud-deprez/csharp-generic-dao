using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Access.Object.Test.Context;
using Data.Access.Object.Test.Entity;
using System.Data.Common;
using System.Collections.Generic;

namespace Data.Access.Object.Test
{
    [TestClass]
    public class RepositoryManagerTest
    {
        IRepositoryManager _repositoryManager;

        [TestInitialize]
        public void testInitialize()
        {
            var dbContext = new MyDbContext();
            _repositoryManager = new RepositoyManager(dbContext);

            #region Create company
            var company = new Company
            {
                Name = "MyCompany"
            };

            //Create company
            _repositoryManager.Repository<Company>().Insert(company);
            _repositoryManager.Save();

            #endregion

            #region Create Skills

            var skills = new List<Skill>();

            skills.Add(new Skill
            {
                Name = "People Management",
                Description = "Get the best of people, manage conflicts",
            });
            skills.Add(new Skill
            {
                Name = "C#", 
                Description = "Programming in C#"
            });
            skills.Add(new Skill
            {
                Name = "F#",
                Description = "Programming in C#"
            });
            skills.Add(new Skill
            {
                Name = "HTML 5 & CSS 3",
                Description = "Basic Web Developer"
            });

            foreach (var s in skills)
            {
                _repositoryManager.Repository<Skill>().Insert(s);
            }

            #endregion

            #region Create Managers

            var managers = new List<Manager>();

            managers.Add(new Manager
            {
                Firstname = "Arnaud",
                LastName = "Deprez",
                Email = @"arnaudeprez@mycompany.com",
                PhoneNumber = @"+321234578900",
                Company = company
            });
            foreach (var s in skills)
            {
                managers[0].Skills.Add(s);
            }

            managers.Add(new Manager
            {
                Firstname = "Jean",
                LastName = "Michel",
                Email = @"jean.michel@mycompany.com",
                PhoneNumber = @"+32123456789",
                Company = company,
                Boss = managers[0]
            });
            managers[1].Skills.Add(skills[0]);

            foreach (var m in managers)
            {
                _repositoryManager.Repository<Manager>().Insert(m);
            }

            #endregion

            #region Create Employees

            var employees = new List<Employee>();

            employees.Add(new Employee
            {
                Firstname = "Poco",
                LastName = "Developer",
                Email = @"poco.developer@mycompany.com",
                Company = company,
                Boss = managers[1]
            });
            employees[0].Skills.Add(skills[1]);

            employees.Add(new Employee
            {
                Firstname = "Functional",
                LastName = "Developer",
                Email = @"functional.developer@mycompany.com",
                Company = company,
                Boss = managers[1]
            });
            employees[1].Skills.Add(skills[2]);

            employees.Add(new Employee
            {
                Firstname = "Web",
                LastName = "Developer",
                Email = @"web.developer@mycompany.com",
                Company = company,
                Boss = managers[1]
            });
            employees[2].Skills.Add(skills[3]);

            foreach (var e in employees)
            {
                _repositoryManager.Repository<Employee>().Insert(e);
            }
            
            #endregion

            //commit
            _repositoryManager.Save();
        }

        [TestCleanup]
        public void testCleanup()
        {
            _repositoryManager.Dispose();
            _repositoryManager = null;
        }
        
        [TestMethod]
        public void CheckDbDatas()
        {
            Assert.AreEqual(1, _repositoryManager.Repository<Company>().Query().Count());
            Assert.AreEqual(4, _repositoryManager.Repository<Skill>().Query().Count());
            Assert.AreEqual(2, _repositoryManager.Repository<Manager>().Query().Count());
            Assert.AreEqual(5, _repositoryManager.Repository<Employee>().Query().Count());

            //check company link
            var companyEnumerator = _repositoryManager.Repository<Company>().Query().Fetch(c => c.Employees).Get().GetEnumerator();
            Assert.IsTrue(companyEnumerator.MoveNext());
            var company = companyEnumerator.Current;
            Assert.AreEqual(5, company.Employees.Count);
            //only primitive types for where condition...
            Assert.AreEqual(5, _repositoryManager.Repository<Employee>().Query().Where(e => e.CompanyId == company.Id).Count());

            //Check Employee links
            var employeeEnumerator = _repositoryManager.Repository<Employee>().Query().Where(e => e.Company.Id == company.Id).Get().GetEnumerator();
            while (employeeEnumerator.MoveNext())
            {
                Assert.IsNotNull(employeeEnumerator.Current.Skills);
                Assert.IsTrue(employeeEnumerator.Current.Skills.Count > 0);
                if (employeeEnumerator.Current.Firstname.Equals("Arnaud"))
                {
                    Assert.IsFalse(employeeEnumerator.Current.BossId.HasValue);
                    Assert.IsNull(employeeEnumerator.Current.Boss);
                    Assert.IsNotNull(employeeEnumerator.Current.Skills);
                    Assert.AreEqual(4, employeeEnumerator.Current.Skills.Count);
                    Assert.IsInstanceOfType(employeeEnumerator.Current, typeof(Manager));
                    Assert.IsNotNull(((Manager)employeeEnumerator.Current).Employees);
                    Assert.AreEqual(1, ((Manager)employeeEnumerator.Current).Employees.Count);
                }
                else
                {
                    Assert.IsTrue(employeeEnumerator.Current.BossId.HasValue);
                    Assert.IsNotNull(employeeEnumerator.Current.Boss);
                    Assert.IsNotNull(employeeEnumerator.Current.Skills);
                    Assert.AreEqual(1, employeeEnumerator.Current.Skills.Count);
                    if (employeeEnumerator.Current.Firstname.Equals("Jean"))
                    {
                        Assert.IsInstanceOfType(employeeEnumerator.Current, typeof(Manager));
                        Assert.IsNotNull(((Manager)employeeEnumerator.Current).Employees);
                        Assert.AreEqual(3, ((Manager)employeeEnumerator.Current).Employees.Count);
                    }
                }
            }

            //Check Manager links
            var managerEnumerator = _repositoryManager.Repository<Manager>().Query().Where(m => m.Company.Id == company.Id).Get().GetEnumerator();
            while (managerEnumerator.MoveNext())
            {
                Assert.IsNotNull(managerEnumerator.Current.Skills);
                Assert.IsTrue(managerEnumerator.Current.Skills.Count > 0);
                if (managerEnumerator.Current.Firstname.Equals("Arnaud"))
                {
                    Assert.IsFalse(managerEnumerator.Current.BossId.HasValue);
                    Assert.IsNull(managerEnumerator.Current.Boss);
                    Assert.IsNotNull(managerEnumerator.Current.Skills);
                    Assert.AreEqual(4, managerEnumerator.Current.Skills.Count);
                    Assert.IsNotNull(managerEnumerator.Current.Employees);
                    Assert.AreEqual(1, managerEnumerator.Current.Employees.Count);
                }
                else
                {
                    Assert.IsTrue(managerEnumerator.Current.BossId.HasValue);
                    Assert.IsNotNull(managerEnumerator.Current.Boss);
                    Assert.IsNotNull(managerEnumerator.Current.Skills);
                    Assert.AreEqual(1, managerEnumerator.Current.Skills.Count);
                    Assert.IsNotNull(managerEnumerator.Current.Employees);
                    Assert.AreEqual(3, managerEnumerator.Current.Employees.Count);
                }
            }
        }
    }
}
