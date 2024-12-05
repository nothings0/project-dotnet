using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DomainModels;
using SV21T1020546.Web.Models;
using System.Buffers;

namespace SV21T1020546.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class ShipperController : Controller
    {
        public int PAGE_SIZE = 5;
        private const string SHIPPER_SEARCH_CONDITION = "ShipperSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH_CONDITION);
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
            var data = CommonDataService.ListOfShippers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            ShipperSearchResult model = new ShipperSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SHIPPER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            var data = new Shipper()
            {
                ShipperID = 0

            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            var data = CommonDataService.GetShipper(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool a = CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetShipper(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);

        }
        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật thông tin người giao hàng";
            //TODO: Kiem tra du lieu dau vao dung hay khong?

            if (String.IsNullOrWhiteSpace(data.ShipperName))
            {
                ModelState.AddModelError(nameof(data.ShipperName), "Tên người giao hàng không được rỗng");
            }

            if (String.IsNullOrWhiteSpace(data.Phone))
            {
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (data.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(data);
                if (id <= 0)
                {
                    ModelState.AddModelError("InvalidPhone", "Số điện thoại bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(data);
                if (!result)
                {
                    ModelState.AddModelError("InvalidPhone", "Số điện thoại bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}