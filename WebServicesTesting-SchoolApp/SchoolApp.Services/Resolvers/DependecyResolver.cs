using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using SchoolApp.Models;
using SchoolApp.Repositories;
using SchoolApp.DataLayer;
using SchoolApp.Services.Controllers;

namespace SchoolApp.Services.Resolvers
{
    public class DependecyResolver : IDependencyResolver
    {
        private static SchoolAppDbContext placesContext = new SchoolAppDbContext();

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(SchoolsController))
            {
                IRepository<School> repository = new SchoolDbRepository(placesContext);
                return new SchoolsController(repository);
            }
            else if (serviceType == typeof(StudentsController))
            {
                IStudentRepository<Student> repository = new StudentDbRepository(placesContext);
                return new StudentsController(repository);
            }
            else
            {
                return null;
            }
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