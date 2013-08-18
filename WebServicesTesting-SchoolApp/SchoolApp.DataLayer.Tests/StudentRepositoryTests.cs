using System;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolApp.DataLayer;
using SchoolApp.Models;
using System.Collections.Generic;

namespace SchoolApp.Repositories.Tests
{
    [TestClass]
    public class StudentRepositoryTests
    {
        public SchoolAppDbContext DbContext { get; set; }
        public IStudentRepository<Student> Repository { get; set; }

        private static TransactionScope tranScope;

        public StudentRepositoryTests()
        {
            this.DbContext = new SchoolAppDbContext();
            this.Repository = new StudentDbRepository(this.DbContext);
        }

        [TestInitialize]
        public void TestInit()
        {
            tranScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            tranScope.Dispose();
        }

        [TestMethod]
        public void AddStudent_WithProperModel()
        {
           Student student = new Student()
           {
               FirstName = "John",
               LastName = "Tomson",
               Age = 15,
               Grade = 9
           };

           Student returnedStudent = this.Repository.Add(student);
           Assert.AreEqual(student.FirstName, returnedStudent.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException))]
        public void AddStudent_WithNotProperModel()
        {
            Student student = new Student()
            {
                LastName = "Tomson",
                Age = 15
            };

            Student returnedStudent = this.Repository.Add(student);
        }

        [TestMethod]
        public void AddStudent_WithProperSchoolId()
        {
            School school = this.DbContext.Schools.Add(new School()
            {
                Name = "Elite College",
                Location = "California"
            });

            Student student = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };

            Student returnedStudent = this.Repository.Add(student, school.Id);
            Assert.AreEqual(student.School.Id, school.Id);
        }

        [TestMethod]
        public void AddStudent_WithNotProperSchoolId()
        {
            Student student = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };

            Student returnedStudent = this.Repository.Add(student, 100);
            Assert.IsNull(returnedStudent);
        }

        [TestMethod]
        public void GetStudents()
        {
            this.Repository.Add(new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            });
            this.Repository.Add(new Student()
            {
                FirstName = "Damian",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            });

            var students = this.Repository.GetAll();

            Assert.AreEqual(2, students.Count());
            Assert.AreEqual("Damian", students.Last().FirstName);
        }

        [TestMethod]
        public void GetStudentById()
        {
            var student = this.Repository.Add(new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            });
            this.Repository.Add(new Student()
            {
                FirstName = "Damian",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            });

            var returnedStudent = this.Repository.Get(student.Id);

            Assert.AreEqual("John", returnedStudent.FirstName);
        }

        [TestMethod]
        public void GetStudentsByMarkSubjectAndValue()
        {
            this.Repository.Add(new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9,
                Marks = new List<Mark>()
                {
                    new Mark()
                    {
                        Subject = "Math",
                        Value = 6.00M
                    }
                }
                
            });
            this.Repository.Add(new Student()
            {
                FirstName = "Damian",
                LastName = "Tomson",
                Age = 15,
                Grade = 9,
                Marks = new List<Mark>()
                {
                    new Mark()
                    {
                        Subject = "Math",
                        Value = 5.00M
                    }
                }
            });
            var student = this.Repository.Add(new Student()
            {
                FirstName = "Eric",
                LastName = "Tomson",
                Age = 15,
                Grade = 9,
                Marks = new List<Mark>()
                {
                    new Mark()
                    {
                        Subject = "Math",
                        Value = 6.00M
                    },
                    new Mark()
                    {
                        Subject = "Physics",
                        Value = 5.00M
                    }
                }
            });

            var students = this.Repository.Get("Math", 6.00M);

            Assert.AreEqual(2, students.Count());
            Assert.AreEqual("Eric", students.Last().FirstName);
        }
    }
}
