using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork ,IDisposable
    {
        private readonly MvcDbcontext _dbcontext;

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public UnitOfWork(MvcDbcontext dbcontext) 
        { 
            EmployeeRepository=new EmployeeRepository(dbcontext);
            DepartmentRepository=new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }

        public int Complete()
        {
            return _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
