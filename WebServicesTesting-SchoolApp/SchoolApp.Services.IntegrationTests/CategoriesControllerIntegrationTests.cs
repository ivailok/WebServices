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
    public class CategoriesControllerIntegrationTests
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
        public void PostStudent_WhenFirstNameIsNull_ShouldReturnStatusCode500()
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
                    FirstName = null
                });

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
