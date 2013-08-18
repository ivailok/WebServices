using System;
using System.Collections.Generic;
using System.Linq;
using SchoolApp.DataLayer;
using SchoolApp.Models;

namespace SchoolApp.Repositories
{
    public class SchoolDbRepository : IRepository<School>
    {
        private SchoolAppDbContext dbContext;

        public SchoolDbRepository(SchoolAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<School> GetAll()
        {
            return this.dbContext.Schools.AsEnumerable();
        }

        public School Get(int id)
        {
            return this.dbContext.Schools.Find(id);
        }

        public School Add(School item)
        {
            School school = this.dbContext.Schools.Add(item);
            this.dbContext.SaveChanges();
            return school;
        }
    }
}
