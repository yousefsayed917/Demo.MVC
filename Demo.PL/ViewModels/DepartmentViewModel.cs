using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "code is required")]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
