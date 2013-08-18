using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Add(T item);
    }
}
