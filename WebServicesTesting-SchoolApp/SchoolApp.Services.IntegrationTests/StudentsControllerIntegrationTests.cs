using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolApp.Models;
using SchoolApp.Repositories;
using Telerik.JustMock;
using System.Collections.Generic;
using System.Net;

namespace SchoolApp.Services.IntegrationTests
{
    [TestClass]
    public class StudentsControllerIntegrationTests
    {
        [TestMethod]
        public void GetAll_WhenOneStudent_ShouldReturnStatusCode200AndNotNullContent()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();
            var models = new List<Student>();
            models.Add(new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            });

            Mock.Arrange(() => mockRepository.GetAll())
                .Returns(() => models.AsQueryable());
            var server = new InMemoryHttpServer<Student>("http://localhost/",
                mockRepository);

            var response = server.CreateGetRequest("api/students");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void PostStudent_WhenFirstNameIsNull_ShouldReturnStatusCode400()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();

            Mock.Arrange(() => mockRepository
                .Add(Arg.Matches<Student>(student => student.FirstName == null)))
                    .Throws<NullReferenceException>();


            var server =
                new InMemoryHttpServer<Student>("http://localhost/", mockRepository);

            var response = server.CreatePostRequest("api/students",
                new Student()
                {
                    LastName = "Tomson",
                    Age = 15,
                    Grade = 9
                });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void PostStudent_WithoutSchoolId_ShouldReturnStatusCode201()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();

            Mock.Arrange(() => mockRepository
                .Add(Arg.Matches<Student>(student =>
                    student.FirstName == "Joshua" &&
                    student.LastName == "Tomson" &&
                    student.Age == 15 &&
                    student.Grade == 9)))
                    .Returns(() => new Student()
                    {
                        FirstName = "Joshua",
                        LastName = "Tomson",
                        Age = 15,
                        Grade = 9
                    });

            var server =
                new InMemoryHttpServer<Student>("http://localhost/", mockRepository);

            var response = server.CreatePostRequest("api/students",
                new Student()
                {
                    FirstName = "Joshua",
                    LastName = "Tomson",
                    Age = 15,
                    Grade = 9
                });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void PostStudent_WithSchoolId_ShouldReturnStatusCode201()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();

            Mock.Arrange(() => mockRepository
                .Add(Arg.Matches<Student>(student =>
                    student.FirstName == "Joshua" &&
                    student.LastName == "Tomson" &&
                    student.Age == 15 &&
                    student.Grade == 9),
                    Arg.Matches<int>(x => x == 1)))
                    .Returns(() => new Student()
                    {
                        FirstName = "Joshua",
                        LastName = "Tomson",
                        Age = 15,
                        Grade = 9
                    });

            var server =
                new InMemoryHttpServer<Student>("http://localhost/", mockRepository);

            var response = server.CreatePostRequest("api/students?schoolId=1",
                new Student()
                {
                    FirstName = "Joshua",
                    LastName = "Tomson",
                    Age = 15,
                    Grade = 9
                });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void GetById_ShouldReturnStatusCode200()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();

            Mock.Arrange(() => mockRepository
                .Get(Arg.Matches<int>(x => x == 1)))
                    .Returns(new Student()
                    {
                        FirstName = "Abraham",
                        LastName = "Tomson",
                        Age = 15,
                        Grade = 9
                    });

            var server =
                new InMemoryHttpServer<Student>("http://localhost/", mockRepository);

            var response = server.CreateGetRequest("api/students/1");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetStudents_ByMarkSubjectAndValue_ShouldReturnStatusCode200()
        {
            var mockRepository = Mock.Create<IStudentRepository<Student>>();
            var models = new List<Student>()
            {
                new Student()
                {
                    FirstName = "Jim",
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
                },
                new Student()
                {
                    FirstName = "Adam",
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
                }
            };

            Mock.Arrange(() => mockRepository
                .Get(Arg.Matches<string>(x => x == "Math"),
                     Arg.Matches<decimal>(x => x == 6.00M)))
                    .Returns(() => models.AsQueryable());

            var server = new InMemoryHttpServer<Student>("http://localhost/", mockRepository);

            var response = server.CreateGetRequest("api/students?subject=Math&value=6.00");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
        }
    }
}
