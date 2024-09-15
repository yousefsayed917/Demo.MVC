using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork,IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Actions
        public IActionResult Index(string SearchString)
        {
            var department = _unitOfWork.DepartmentRepository.GetAll();
            var MappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            if (!string.IsNullOrEmpty(SearchString))
                department = department.Where(e => e.Name.Contains(SearchString)).ToList();
            return View(MappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentview)
        {
            if (ModelState.IsValid)//server side validtion
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentview);
                _departmentRepository.Add(MappedDepartment);
                TempData["AlertMessage"] = "Department Added Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentview);
        }
        public IActionResult Details(int? id , string ViewName="Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var department = _departmentRepository.GetById(id.Value);
            if (department == null)
                return NotFound();
            var MappedDepartment=_mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName,MappedDepartment);
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
            return Details(id,"Edit");
        }
        [HttpPost]
        public IActionResult Edit(DepartmentViewModel departmentview, [FromRoute]int id)
        {

            if (ModelState.IsValid)//server side validtion
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel,Department>(departmentview);
                    _departmentRepository.Update(MappedDepartment);
                    TempData["AlertMessage"] = "Department Updated Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentview);
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
        public IActionResult Delete(DepartmentViewModel departmentview, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentview);
                    _departmentRepository.Delete(MappedDepartment);
                    TempData["AlertMessage"] = "Department Deleted Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
