using System;
using System.Collections.Generic;
using System.Linq;
using SchoolApp.Repositories;
using SchoolApp.Models;

namespace SchoolApp.Services.Tests
{
    public class FakeRepository : IStudentRepository<Student>
    {
        private IList<Student> entities = new List<Student>();
        
        public IEnumerable<Student> GetAll()
        {
            return this.entities;
        }

        public Student Get(int id)
        {
            if (id < 0 || id >= this.entities.Count)
            {
                return null;
            }
            return this.entities[id];
        }

        public Student Add(Student item)
        {
            this.entities.Add(item);
            return item;
        }

        public IEnumerable<Student> Get(string subject, decimal value)
        {
            IList<Student> students = new List<Student>();

            foreach (var entity in this.entities)
            {
                foreach (var mark in entity.Marks)
                {
                    if (mark.Subject == subject && mark.Value == value)
                    {
                        students.Add(entity);
                        break;
                    }
                }
            }

            return students;
        }

        public Student Add(Student item, int schoolId)
        {
            item.School = new School()
            {
            };
            item.School.Id = schoolId;
            this.entities.Add(item);
            return item;
        }
    }
}
