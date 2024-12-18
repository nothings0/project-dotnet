using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DomainModels;
using SV21T1020546.Web.AppCodes;
using SV21T1020546.Web.Models;
using System.Globalization;

namespace SV21T1020546.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR}")]
    public class EmployeeController : Controller
    {
        public int PAGE_SIZE = 10;
        private const string EMPLOYEE_SEARCH_CONDITION = "EmployeeSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH_CONDITION);
            if (condition == null)
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            return View(condition);
        }
        public IActionResult Search(PaginationSearchInput condition)
        {
            int rowCount;
            var data = CommonDataService.ListOfEmployees(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            EmployeeSearchResult model = new EmployeeSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var data = new Employee()
            {
                EmployeeID = 0,
                IsWorking = false
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool a = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetEmployee(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }
        [HttpPost]
        public IActionResult Save(Employee data, string _birthDate, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";

            if (String.IsNullOrWhiteSpace(data.FullName))
            {
                ModelState.AddModelError(nameof(data.FullName), "*");
            }

            if (String.IsNullOrWhiteSpace(data.Email))
            {
                ModelState.AddModelError(nameof(data.Email), "*");
            }

            if (String.IsNullOrWhiteSpace(data.Address))
            {
                data.Address = "";
            }
            if (String.IsNullOrWhiteSpace(data.Phone))
            {
                data.Phone = "";
            }


            DateTime? d = _birthDate.ToDateTime();

            if (d != null)
            {
                if (d.Value.Year < 1753)
                {
                    ModelState.AddModelError("ERROR_BIRTHDATE", "Vui lòng nhập ngày sinh sau năm 1753");
                }
                data.BirthDate = d.Value;
            }
            else
            {
                ModelState.AddModelError(nameof(data.BirthDate), "*");
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\employees", fileName);

                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    uploadPhoto.CopyTo(stream);
                }

                data.Photo = fileName;
            }
            //TODO: Kiem tra du lieu dau vao dung hay khong?
            if (data.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(data);
                if (id <= 0)
                {
                    ModelState.AddModelError("InvalidEmail", "Email bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(data);
                if (!result)
                {
                    ModelState.AddModelError("InvalidEmail", "Email bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}