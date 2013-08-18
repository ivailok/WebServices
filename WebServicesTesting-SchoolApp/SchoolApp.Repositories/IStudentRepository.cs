using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp.Repositories
{
    public interface IStudentRepository<T> : IRepository<T>
    {
        IEnumerable<T> Get(string subject, decimal value);
        T Add(T item, int schoolId);
    }
}
