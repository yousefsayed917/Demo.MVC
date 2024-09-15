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
        public void Add(T item);
        public void Update(T item);
        public void Delete(T item);
    }
}
