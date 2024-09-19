using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Actions
        public async Task<IActionResult> Index(string SearchString)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var MappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            if (!string.IsNullOrEmpty(SearchString))
                MappedDepartment = MappedDepartment.Where(e => e.Name.Contains(SearchString)).ToList();
            return View(MappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(DepartmentViewModel departmentview)
        {
            if (ModelState.IsValid)//server side validtion
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentview);
                await _unitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
                int result =await _unitOfWork.CompleteAsync();
                if (result > 0)
                    TempData["AlertMessage"] = "Department Added Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentview);
        }
        public async Task<IActionResult> DetailsAsync(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department == null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, MappedDepartment);
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            //if (id == null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();

            //return View(department);
            return await DetailsAsync(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(DepartmentViewModel departmentview, [FromRoute] int id)
        {

            if (ModelState.IsValid)//server side validtion
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentview);
                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                    int result =await _unitOfWork.CompleteAsync();
                    if (result > 0)
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
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            //if (id == null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();
            //return View(department);
            return await DetailsAsync(id, "Delete");
        }
        [HttpPost]
        
        public async Task<IActionResult> DeleteAsync(DepartmentViewModel departmentview, [FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentview);
                    _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                    int result =await _unitOfWork.CompleteAsync();
                    if (result > 0)
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
