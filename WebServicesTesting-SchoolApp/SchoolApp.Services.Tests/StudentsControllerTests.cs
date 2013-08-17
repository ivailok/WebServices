using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using SchoolApp.Repositories;
using Telerik.JustMock;
using SchoolApp.Models;
using SchoolApp.Services.Controllers;
using SchoolApp.Services.Models;

namespace SchoolApp.Services.Tests
{
    [TestClass]
    public class StudentsControllerTests
    {
        private void SetupController(ApiController controller)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/students");
            var route = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var routeData = new HttpRouteData(route);
            routeData.Values.Add("id", RouteParameter.Optional);
            routeData.Values.Add("controller", "students");
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
        }

        [TestMethod]
        public void Add_WhenModelIsValid_ShouldAddTheStudent()
        {
            bool isItemAdded = false;
            var repository = Mock.Create<IStudentRepository<Student>>();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };

            Mock.Arrange(() => repository.Add(Arg.IsAny<Student>()))
                .DoInstead(() => isItemAdded = true)
                .Returns(studentEntity);

            var controller = new StudentsController(repository);
            SetupController(controller);

            controller.Post(studentEntity);
            Assert.IsTrue(isItemAdded);
        }

        [TestMethod]
        public void Add_WhenSchoolIsSet_ShouldAddTheStudentLinkedToTheSchool()
        {
            var repository = new FakeRepository();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9,
                Marks = new List<Mark>() {
                    new Mark()
                    {
                        Subject = "Math",
                        Value = 6.00M
                    }
                }
            };

            var controller = new StudentsController(repository);
            SetupController(controller);

            HttpResponseMessage message = controller.Post(studentEntity, 1);

            Assert.IsNotNull(message.Content);
        }

        [TestMethod]
        public void GetAllStudents_WhenSingleStudentInRepository_ShouldReturnSingleStudent()
        {
            var repository = new FakeRepository();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity);

            var controller = new StudentsController(repository);
            IEnumerable<StudentModel> studentModels = controller.Get();
            Assert.IsTrue(studentModels.Count() == 1);
            Assert.AreEqual(studentEntity.FirstName, studentModels.First().FirstName);
        }

        [TestMethod]
        public void GetAllStudents_WhenRepositoryIsEmpty_ShouldReturnNoStudent()
        {
            var repository = new FakeRepository();

            var controller = new StudentsController(repository);
            IEnumerable<StudentModel> studentModels = controller.Get();
            Assert.IsTrue(studentModels.Count() == 0);
        }

        [TestMethod]
        public void GetAllStudents_WhenMultipleStudentsInRepository_ShouldReturnMultipleStudents()
        {
            var repository = new FakeRepository();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity);

            var studentEntity2 = new Student()
            {
                FirstName = "Angel",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity2);

            var studentEntity3 = new Student()
            {
                FirstName = "William",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity3);

            var controller = new StudentsController(repository);
            IEnumerable<StudentModel> studentModels = controller.Get();
            Assert.IsTrue(studentModels.Count() == 3);
            Assert.AreEqual(studentEntity3.FirstName, studentModels.Last().FirstName);
        }

        [TestMethod]
        public void GetSingleStudent_WhenStudentInRepository_ShouldReturnTheStudent()
        {
            var repository = new FakeRepository();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity);

            var studentEntity2 = new Student()
            {
                FirstName = "Angel",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity2);

            var studentEntity3 = new Student()
            {
                FirstName = "William",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity3);

            var controller = new StudentsController(repository);
            StudentModel studentModel = controller.Get(1);
            Assert.AreEqual(studentEntity2.FirstName, studentModel.FirstName);
        }

        [TestMethod]
        public void GetSingleStudent_WhenStudentNotInRepository_ShouldReturnNull()
        {
            var repository = new FakeRepository();

            var studentEntity = new Student()
            {
                FirstName = "John",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity);

            var studentEntity2 = new Student()
            {
                FirstName = "Angel",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity2);

            var studentEntity3 = new Student()
            {
                FirstName = "William",
                LastName = "Tomson",
                Age = 15,
                Grade = 9
            };
            repository.Add(studentEntity3);

            var controller = new StudentsController(repository);
            StudentModel studentModel = controller.Get(4);
            Assert.IsNull(studentModel);
        }
    }
}
