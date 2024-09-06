using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenaricRepository<T> 
    {
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public int Add(T item);
        public int Update(T item);
        public int Delete(T item);
    }
}
