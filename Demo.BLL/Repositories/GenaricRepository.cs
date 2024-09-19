using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly MvcDbcontext _dbcontext;

        public GenaricRepository(MvcDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddAsync(T item)
        {
           await _dbcontext.AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbcontext.Remove(item);
            //  async بتاعت الاوبجيكت بس عشان كدا مبتشتغلش  state هي و الابديت مبيعملوش حاجة في الداتا بيز هما بيغيرو ف ال 
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //if (typeof(T)==typeof(Employee))
            //    return (IEnumerable<T>)await _dbcontext.Employees.Include(e=>e.Department).ToListAsync();//Egarloading
            return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)

            => await _dbcontext.Set<T>().FindAsync(id);

        public void Update(T item)
        {
            _dbcontext.Update(item);
        }
    }
}
