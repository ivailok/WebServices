using System.Collections.Generic;
using SchoolApp.DataLayer;
using SchoolApp.Models;
using System;
using System.Linq;

namespace SchoolApp.Repositories
{
    public class StudentDbRepository : IStudentRepository<Student>
    {
        private SchoolAppDbContext dbContext;

        public StudentDbRepository(SchoolAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Student> GetAll()
        {
            return this.dbContext.Students.AsEnumerable();
        }

        public Student Get(int id)
        {
            return this.dbContext.Students.Find(id);
        }

        public Student Add(Student item)
        {
            Student student = this.dbContext.Students.Add(item);
            this.dbContext.SaveChanges();
            return student;
        }

        public IEnumerable<Student> Get(string subject, decimal value)
        {
            var students =
                from student in this.dbContext.Students
                where student.Marks.Where(x => x.Subject == subject && x.Value == value).Count() > 0
                select student;

            return students;
        }

        public Student Add(Student item, int schoolId)
        {
            School school = this.dbContext.Schools.Find(schoolId);
            if (school == null)
            {
                return null;
            }
            Student student = this.dbContext.Students.Add(item);
            student.School = school;
            this.dbContext.SaveChanges();
            return student;
        }
    }
}
