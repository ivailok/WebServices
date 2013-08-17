using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp.Models
{
    public class School
    {
        private ICollection<Student> students;

        public School()
        {
            this.students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Student> Students
        {
            get
            {
                return this.students;
            }
            set
            {
                this.students = value;
            }
        }
    }
}
