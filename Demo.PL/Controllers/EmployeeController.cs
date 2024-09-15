using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(/*IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
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
            var employee = _unitOfWork.EmployeeRepository.GetAll();
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
            if (!string.IsNullOrEmpty(SearchString))
                employee = employee.Where(e => e.Name.Contains(SearchString)).ToList();
            return View(MappedEmployee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Department = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeview)
        {
            if (ModelState.IsValid)//server side validtion
            {
                //Manual Mapping 
                //var MappedEmployee = new Employee
                //{
                //    Name = employeeview.Name,
                //    Age = employeeview.Age,
                //    Address = employeeview.Address,
                //    PhoneNumber = employeeview.PhoneNumber,
                //    Email = employeeview.Email,
                //};
                var MappedEmployee=_mapper.Map<EmployeeViewModel,Employee>(employeeview);
                 _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                int result=_unitOfWork.Complete(); 
                if (result > 0)
                    TempData["AlertMessage"] = "Employee Added Successfuly";
                //1-temp data =>key value pair[dictionary] من الاكشن للأكشن
                //transfer data from action to action //.net framework 4.0
                return RedirectToAction(nameof(Index));
            }
            return View(employeeview);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee == null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee,EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
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
        public IActionResult Edit(EmployeeViewModel employeeview, [FromRoute] int id)
        {

            if (ModelState.IsValid)//server side validtion
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeview);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    int result = _unitOfWork.Complete();
                    if (result > 0)
                        TempData["AlertMessage"] = "Employee Updated Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeview);
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
        public IActionResult Delete(EmployeeViewModel employeeview, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeview);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                    int result = _unitOfWork.Complete();
                    if (result > 0)
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
