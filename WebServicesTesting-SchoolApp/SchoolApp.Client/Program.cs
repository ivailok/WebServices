using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolApp.Repositories;
using SchoolApp.Models;

namespace SchoolApp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IStudentRepository<Student> rep = new StudentDbRepository(new SchoolApp.DataLayer.SchoolAppDbContext());
            rep.Add(new Student()
            {
                FirstName = "Ivo",
                LastName = "Petrov",
                Age = 14,
                Grade = 8,
                Marks = new List<Mark>() { new Mark() { Subject = "Math", Value = 10.00M} }
            });

            var students = rep.Get("Math", 10.00M);
            foreach (var student in students)
            {
                Console.WriteLine(student.LastName);
            }
        }
    }
}
