using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index(string SearchString)
        {
            var employee = _employeeRepository.GetAll();
            if (!string.IsNullOrEmpty(SearchString))
                employee=employee.Where(e=>e.Name.Contains(SearchString)).ToList();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)//server side validtion
            {
                _employeeRepository.Add(employee);
                TempData["AlertMessage"] = "Employee Added Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var employee = _employeeRepository.GetById(id.Value);
            if (employee == null)
                return NotFound();
            return View(ViewName, employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id == null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();

            //return View(department);
            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit(Employee employee, [FromRoute] int id)
        {

            if (ModelState.IsValid)//server side validtion
            {
                try
                {
                    _employeeRepository.Update(employee);
                    TempData["AlertMessage"] = "Employee Updated Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id == null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();
            //return View(department);
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee employee, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Delete(employee);
                    TempData["AlertMessage"] = "Employee Deleted Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
