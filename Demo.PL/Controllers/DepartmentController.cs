using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #region Actions
        public IActionResult Index(string SearchString)
        {
            var department = _departmentRepository.GetAll();
            if (!string.IsNullOrEmpty(SearchString))
                department = department.Where(e => e.Name.Contains(SearchString)).ToList();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)//server side validtion
            {
                _departmentRepository.Add(department);
                TempData["AlertMessage"] = "Department Added Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public IActionResult Details(int? id , string ViewName="Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var department = _departmentRepository.GetById(id.Value);
            if (department == null)
                return NotFound();
            return View(ViewName,department);
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
        public IActionResult Edit(Department department, [FromRoute]int id)
        {

            if (ModelState.IsValid)//server side validtion
            {
                try
                {
                    _departmentRepository.Update(department);
                    TempData["AlertMessage"] = "Department Updated Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
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
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Delete(department);
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
