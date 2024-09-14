using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Mane Is Required")]
        [MaxLength(50,ErrorMessage ="Max Length Is 50 Chars")]
        [MinLength(5,ErrorMessage ="Min Length Is 5 Chars")]
        public string Name { get; set; }
        [Range(22,45,ErrorMessage ="Age Must Be In Range From 22 To 45")]
        public int Age { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address {  get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive {  get; set; }
        [EmailAddress]
        public string Email {  get; set; }
        [Phone]
        public string PhoneNumber {  get; set; }
        public DateTime HireDate {  get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [InverseProperty("Employees")]
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }



    }
}
