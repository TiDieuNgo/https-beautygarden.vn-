using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DevExpress.DataAccess.Native.Data;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Models;
using DataColumn = System.Data.DataColumn;
using DataTable = System.Data.DataTable;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class ThongBaoKhiCoHangController : BaseController
    {
        private readonly IThongBaoKhiCoHangRepository _thongBaoKhiCoHangRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IDetailMenuCommentRepository _detailMenuCommentRepository;
        private const int pageSize = 50;
        private readonly HttpContextBase _httpContext;
        public ThongBaoKhiCoHangController(IThongBaoKhiCoHangRepository thongBaoKhiCoHangRepository, IMenuRepository menuRepository, IDetailMenuCommentRepository detailMenuCommentRepository, HttpContextBase httpContext)
        {
            _thongBaoKhiCoHangRepository = thongBaoKhiCoHangRepository;
            _menuRepository = menuRepository;
            _detailMenuCommentRepository = detailMenuCommentRepository;
            _httpContext = httpContext;
        }

        public ActionResult Index(string from, string to, int page = 1)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.ThongBaoKhiCoHang_View)))
                return View("_NoAuthor");

            int pageNumber = page;
            DateTime start = !string.IsNullOrEmpty(from) ? ConvertToShortDay(from) : DateTime.Today;  
            DateTime end = !string.IsNullOrEmpty(to) ? ConvertToShortDay(to) : DateTime.Today;
            DateTime startday = StartOfDay(start);
            DateTime endDay = EndOfDay(end);
            IList<ThongBaoKhiCoHang> hoaDons =
                _thongBaoKhiCoHangRepository.GetMany(o => o.Created >= start && o.Created <= endDay).ToList();

            ThongBaoKhiHetHangModel model = new ThongBaoKhiHetHangModel()
                                            {
                                                From = start.ToString("dd/MM/yyyy"),
                                                To = endDay.ToString("dd/MM/yyyy"),
                                                ThongBaoKhiCoHangs = hoaDons.ToPagedList(pageNumber, pageSize),
                                                page = page,
                                                SoLuong = hoaDons.Count()
                                            };
            return View("Index", model);
        }

        public ActionResult ConHang(string from, string to, int page = 1)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.ThongBaoKhiCoHang_View)))
                return View("_NoAuthor");
            int pageNumber = page;
            DateTime start = !string.IsNullOrEmpty(from) ? ConvertToShortDay(from) : DateTime.Today;
            DateTime end = !string.IsNullOrEmpty(to) ? ConvertToShortDay(to) : DateTime.Today;
            DateTime startday = StartOfDay(start);
            DateTime endDay = EndOfDay(end);
            //lọc những sản phẩm còn hàng
            IList<ThongBaoKhiCoHang> proCoHangs = _thongBaoKhiCoHangRepository.GetSanPhamConHangs().Where(o => o.Created >= start && o.Created <= endDay).ToList();
            ThongBaoKhiHetHangModel model = new ThongBaoKhiHetHangModel()
                                            {
                                                From = start.ToString("dd/MM/yyyy"),
                                                To = endDay.ToString("dd/MM/yyyy"),
                                                ThongBaoKhiCoHangs = proCoHangs.ToPagedList(pageNumber, pageSize),
                                                page = page,
                                                SoLuong = proCoHangs.Count()
                                            };
            return View("ConHang", model);
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
            var Menus = _thongBaoKhiCoHangRepository.GetAll().ToList();
            foreach (var product in Menus)
            {
                string hoten = product.FullName;
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
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongBaoKhiCoHangs.xlsx");
                }
            }
        }
        public class ThongBaoKhiHetHangModel
        {
            public int SoLuong { get; set; }
            public string TenSanPham { get; set; }
            public IPagedList<ThongBaoKhiCoHang> ThongBaoKhiCoHangs { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public int stt { get; set; }
            public int page { get; set; }
        }
       
        public DateTime StartOfDay(DateTime source)
        {
            return Convert.ToDateTime(source.ToShortDateString());
        }

        public DateTime EndOfDay(DateTime source)
        {
            return Convert.ToDateTime(source.ToShortDateString() + " 11:59 PM");
        }
        public DateTime ConvertToShortDay(string input)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy HH:mm";
            dtfi.DateSeparator = "/";

            return Convert.ToDateTime(Regex.Replace(input,
                @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4})\b",
                "${month}/${day}/${year}"), dtfi);
        }
    }
}
