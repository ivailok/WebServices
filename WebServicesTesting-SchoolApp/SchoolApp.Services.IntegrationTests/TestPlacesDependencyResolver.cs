using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using SchoolApp.Repositories;
using SchoolApp.Services.Controllers;
using SchoolApp.Models;

namespace SchoolApp.Services.IntegrationTests
{
    public class TestPlacesDependencyResolver<T> : IDependencyResolver
    {
        private IStudentRepository<T> repository;

        public IStudentRepository<T> Repository
        {
            get
            {
                return this.repository;
            }
            set
            {
                this.repository = value;
            }
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(StudentsController))
            {
                return new StudentsController(this.Repository as IStudentRepository<Student>);
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {

        }
    }
}
