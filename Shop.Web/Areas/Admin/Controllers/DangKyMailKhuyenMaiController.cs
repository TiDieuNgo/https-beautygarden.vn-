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

namespace Shop.Admin.Controllers
{
       // [ShopAuthorize]
    public class DangKyMailKhuyenMaiController : Controller
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DangKyMailKhuyenMaiController(IEmailRepository emailRepository, IUnitOfWork unitOfWork)
        {
            _emailRepository = emailRepository;
            _unitOfWork = unitOfWork;
        }

        private const int pageSize = 20;
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Email> tags = _emailRepository.GetAll().OrderByDescending(o => o.SDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("NhanMail",data);
        }
       public ActionResult delete(int id)
       {
           var idbg = _emailRepository.GetById(id);
           _emailRepository.Delete(idbg);
           _unitOfWork.Commit();
           return RedirectToAction("Index");
       }
        [HttpPost]
        public FileResult ExportExcel()
        {
            ShopDataContex db = new ShopDataContex();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[1]
                                {
                                    new DataColumn("Email")
                                });
            var Menus = _emailRepository.GetAll().ToList();
            foreach (var product in Menus)
            {
                string email = product.Emails;
                dt.Rows.Add(email);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NhanTinKhuyenMai.xlsx");
                }
            }
        }

    }
}
