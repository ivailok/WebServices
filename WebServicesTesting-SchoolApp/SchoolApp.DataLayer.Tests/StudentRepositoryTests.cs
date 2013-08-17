using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolApp.Models;
using SchoolApp.Repositories;

namespace SchoolApp.DataLayer.Tests
{
    [TestClass]
    public class StudentRepositoryTests
    {
        [TestMethod]
        public void AddStudent_WithProperModel()
        {
            using (SchoolAppDbContext dbContext = new SchoolAppDbContext())
            {
                IStudentRepository<Student> repository = new StudentDbRepository(dbContext);

                Student student = new Student()
                {
                    FirstName = "John",
                    LastName = "Tomson",
                    Age = 15,
                    Grade = 9
                };

                Student returnedStudent = repository.Add(student);
                Assert.AreEqual(student.FirstName, returnedStudent.FirstName);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException))]
        public void AddStudent_WithNotProperModel()
        {
            using (SchoolAppDbContext dbContext = new SchoolAppDbContext())
            {
                IStudentRepository<Student> repository = new StudentDbRepository(dbContext);

                Student student = new Student()
                {
                    LastName = "Tomson",
                    Age = 15
                };

                Student returnedStudent = repository.Add(student);
            }
        }

        [TestMethod]
        public void AddStudent_WithProperSchoolId()
        {
            using (SchoolAppDbContext dbContext = new SchoolAppDbContext())
            {
                IStudentRepository<Student> repository = new StudentDbRepository(dbContext);

                School school = dbContext.Schools.Add(new School()
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

                Student returnedStudent = repository.Add(student, school.Id);
                Assert.AreEqual(student.School.Id, school.Id);
            }
        }

        [TestMethod]
        public void AddStudent_WithNotProperSchoolId()
        {
            using (SchoolAppDbContext dbContext = new SchoolAppDbContext())
            {
                IStudentRepository<Student> repository = new StudentDbRepository(dbContext);

                Student student = new Student()
                {
                    FirstName = "John",
                    LastName = "Tomson",
                    Age = 15,
                    Grade = 9
                };

                Student returnedStudent = repository.Add(student, 100);
                Assert.IsNull(returnedStudent);
            }
        }
    }
}
