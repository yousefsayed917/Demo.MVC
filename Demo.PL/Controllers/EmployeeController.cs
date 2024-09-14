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
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index(string SearchString)
        {
            //الفرق مابنهم بيظهر لما اخزنهم في فاريبول
            //ViewData["Message"] = "hello from view date";
            //1-view data =>key value pair[dictionary] من الاكشن للفيو 
            //transfer data from controller[action]to its view //.net framework 3.5
            //ViewBag.Message = "hello from view bag";
            //1-view bag =>key value pair[dictionary] من الاكشن للفيو 
            //transfer data from controller[action]to its view //.net framework 4.0
            var employee = _employeeRepository.GetAll();
            if (!string.IsNullOrEmpty(SearchString))
                employee = employee.Where(e => e.Name.Contains(SearchString)).ToList();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Department = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)//server side validtion
            {
                int result = _employeeRepository.Add(employee);
                if (result > 0)
                    TempData["AlertMessage"] = "Employee Added Successfuly";
                //1-temp data =>key value pair[dictionary] من الاكشن للأكشن
                //transfer data from action to action //.net framework 4.0
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
