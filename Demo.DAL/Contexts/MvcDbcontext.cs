using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcDbcontext:DbContext
    {
        public MvcDbcontext(DbContextOptions<MvcDbcontext> options) :base(options)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server=JOO ; Database=MvcApp ; Trusted_Connection=true");
        //}
       public DbSet<Department> Departments { get; set; }
       public DbSet<Employee> Employees { get; set; }
    }
}
