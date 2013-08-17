using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolApp.Repositories;
using SchoolApp.Services.Models;

namespace SchoolApp.Services.Controllers
{
    public class SchoolsController : ApiController
    {
        IRepository<School> repository;

        public SchoolsController(IRepository<School> repository)
        {
            this.repository = repository;
        }

        // GET api/schools
        public IEnumerable<SchoolModel> Get()
        {
            var schoolEntities = this.repository.GetAll();

            var schools =
                from school in schoolEntities
                select new SchoolModel()
                {
                    Location = school.Location,
                    Name = school.Name
                };

            return schools;
        }

        // GET api/schools/5
        public SchoolModel Get(int id)
        {
            var school = this.repository.Get(id);

            return new SchoolModel()
            {
                Location = school.Location,
                Name = school.Name
            };
        }

        // POST api/schools
        public HttpResponseMessage Post([FromBody]School school)
        {
            if (ModelState.IsValid)
            {
                School schoolEntity = this.repository.Add(school);
                if (schoolEntity != null)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, school);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = school.Id }));
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
