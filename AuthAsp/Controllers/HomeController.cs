using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthAsp.Models;
using AuthAsp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AuthAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return View(employeeRepository.GetEmployees());
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadingImage(model);
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                employeeRepository.Add(employee);

                return RedirectToAction("details", new { id = employee.Id });
            }
            return View();
        }



        public ActionResult Details(int id)
        {
            return View(employeeRepository.GetEmployee(id));
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            EditEmployeeViewModel viewModel = new EditEmployeeViewModel
            {
                Id = employee.Id,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath,
                Name = employee.Name
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = employeeRepository.GetEmployee(model.Id);

                if (model.PhotoPath != null) {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, 
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadingImage(model);
                }

                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                employeeRepository.Update(employee);

                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadingImage(CreateEmployeeViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoPath != null)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoPath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
