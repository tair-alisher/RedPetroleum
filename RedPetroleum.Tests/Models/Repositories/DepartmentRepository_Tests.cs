using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using RedPetroleum.Models.Entities;
using RedPetroleum.Models.Repositories;
using RedPetroleum.Models;


namespace RedPetroleum.Tests.Models.Repositories
{
    [TestClass]
    public class DepartmentRepository_Tests
    {
        DepartmentRepository DepRepo;
        List<Department> departmentsList;

        [TestInitialize]
        public void TestInitialize()
        {
            departmentsList = new List<Department>
            {
                new Department
                {
                    DepartmentId = new Guid(),
                    Name = "Отдел разработки",
                    ParentId = null
                },
                new Department
                {
                    DepartmentId = new Guid(),
                    Name = "Отдел маркетинга",
                    ParentId = null
                }
            };

            departmentsList.Add(new Department
            {
                DepartmentId = new Guid(),
                Name = "Отдел программирования",
                ParentId = departmentsList.ElementAt(0).DepartmentId
            });
            departmentsList.Add(new Department
            {
                DepartmentId = new Guid(),
                Name = "Отдел дизайна",
                ParentId = departmentsList.ElementAt(0).DepartmentId
            });

            IQueryable<Department> departments = departmentsList.AsQueryable();

            Mock<DbSet<Department>> mockSet = new Mock<DbSet<Department>>();
            mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(departments.Provider);
            mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(departments.Expression);
            mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(departments.ElementType);
            mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(departments.GetEnumerator());

            Mock<ApplicationDbContext> mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(d => d.Departments).Returns(mockSet.Object);

            DepRepo = new DepartmentRepository(mockContext.Object);
        }

        [TestMethod]
        public void Create_Test()
        {
            // Arrange
            int DepSetCountBeforeAct = departmentsList.Count();
            Department newDep = new Department
            {
                DepartmentId = new Guid(),
                Name = "Отдел закупок",
                ParentId = null
            };

            // Act
            DepRepo.Create(newDep);

            // Assert
            Assert.AreNotEqual(DepSetCountBeforeAct, DepRepo.GetAll().Count());
        }
    }
}
