using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SchoolApp.Models;
using SchoolApp.Repositories;
using SchoolApp.Services.Models;
using System.Net.Http;
using System.Net;

namespace SchoolApp.Services.Controllers
{
    public class StudentsController : ApiController
    {
        private IStudentRepository<Student> repository;

        public StudentsController(IStudentRepository<Student> repository)
        {
            this.repository = repository;
        }

        // GET api/students
        public IEnumerable<StudentModel> Get()
        {
            var studentEntities = this.repository.GetAll();

            var students = 
                from student in studentEntities
                select new StudentModel()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    Grade = student.Grade
                };

            return students;
        }

        // GET api/students/5
        public StudentModel Get(int id)
        {
            var student = this.repository.Get(id);
            if (student == null)
            {
                return null;
            }

            return new StudentModel()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    Grade = student.Grade
                };
        }

        //GET api/students?subject=math&value=5.00
        public IEnumerable<StudentModel> Get(string subject, decimal value)
        {
            var studentEntities = this.repository.Get(subject, value);

            var students =
                from student in studentEntities
                select new StudentModel()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    Grade = student.Grade
                };

            return students;
        }

        // POST api/students
        public HttpResponseMessage Post([FromBody]Student student)
        {
            if (ModelState.IsValid)
            {
                Student schoolEntity = this.repository.Add(student);
                if (schoolEntity != null)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new StudentModel()
                        {
                            FirstName = schoolEntity.FirstName,
                            LastName = schoolEntity.LastName,
                            Age = schoolEntity.Age,
                            Grade = schoolEntity.Grade
                        });
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = student.Id }));
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrond data in model.");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model.");
            }
        }

        // POST api/students?schoolId=1
        public HttpResponseMessage Post([FromBody]Student student, int schoolId)
        {
            if (ModelState.IsValid)
            {
                Student schoolEntity = this.repository.Add(student, schoolId);
                if (schoolEntity != null)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new StudentModel()
                    {
                        FirstName = schoolEntity.FirstName,
                        LastName = schoolEntity.LastName,
                        Age = schoolEntity.Age,
                        Grade = schoolEntity.Grade
                    });
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = student.Id }));
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrond data in model.");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model.");
            }
        }
    }
}