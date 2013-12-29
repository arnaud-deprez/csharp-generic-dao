using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Access.Object.Test.Context;
using Data.Access.Object.Test.Entity;
using System.Data.Common;

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
            
            //Create company
            _repositoryManager.Repository<Company>().Insert(new Company
            {
                Name = "MyCompany"
            });

            #endregion

            #region Create Skills

            _repositoryManager.Repository<Skill>().Insert(new Skill
            {
                Name = "People Management",
                Description = "Get the best of people, manage conflicts",
            });
            _repositoryManager.Repository<Skill>().Insert(new Skill
            {
                Name = "C#", 
                Description = "Programming in C#"
            });
            _repositoryManager.Repository<Skill>().Insert(new Skill
            {
                Name = "F#",
                Description = "Programming in C#"
            });
            _repositoryManager.Repository<Skill>().Insert(new Skill
            {
                Name = "HTML 5 & CSS 3",
                Description = "Basic Web Developer"
            });

            #endregion

        }

        [TestCleanup]
        public void testCleanup()
        {
            _repositoryManager.Dispose();
            _repositoryManager = null;
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            _repositoryManager.Repository<Company>().Insert(new Company()
            {
                Name = "Watchup"
            });
            _repositoryManager.Save();

            Assert.AreEqual(1, _repositoryManager.Repository<Company>().Query().Count());
        }
    }
}
