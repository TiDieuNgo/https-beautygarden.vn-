using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class KhachHangLienHeController : Controller
    {
        private readonly IAccMember298Repository _accMember298Repository;
        private readonly IUnitOfWork _unitOfWork;
        public KhachHangLienHeController(IAccMember298Repository accMember298Repository, IUnitOfWork unitOfWork)
        {
            _accMember298Repository = accMember298Repository;
            _unitOfWork = unitOfWork;
        }

        private const int pageSize = 20;
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<AccMember298> tags = _accMember298Repository.GetAll().OrderByDescending(o => o.sDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            ViewData["thanhvien"] = tags.Count();

            return View("Lienhe", data);
        }
        public ActionResult delete(int id)
        {
            var idbg = _accMember298Repository.GetById(id);
            _accMember298Repository.Delete(idbg);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult thongke()
        {
            IList<AccMember298> tags = _accMember298Repository.GetAll().OrderByDescending(o => o.sDate).ToList();

            ViewData["thanhvien"] = tags.Count();

            return View("ThongKe");
        }
        [HttpPost]
        public FileResult ExportExcel()
        {

            ShopDataContex db = new ShopDataContex();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3]
                                {
                                    new DataColumn("Họ tên"),
                                    new DataColumn("Số điện thoại"),
                                    new DataColumn("Email")
                                });
            var Menus = _accMember298Repository.GetAll().ToList();
            foreach (var product in Menus)
            {
                string hoten = product.Fullname;
                string std = product.Phone;
                string email = product.Email;
                dt.Rows.Add(hoten, std, email);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThanhVienBG.xlsx");
                }
            }
        }
    }
}
