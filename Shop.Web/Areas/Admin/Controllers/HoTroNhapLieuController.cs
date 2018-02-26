using System;
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
using Shop.Web.Areas.Admin.Models;

namespace Shop.Admin.Controllers
{
   // [ShopAuthorize]
    public class HoTroNhapLieuController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private const int pageSize = 50;
        private readonly IUnitOfWork _unitOfWork;

        public HoTroNhapLieuController(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(int? sx, int page = 1)
        {
            int pageNumber = page;
            if (!sx.HasValue)//nếu sản phẩm không có giá trị lọc thì vẫn hiển thị bình thường
            {
                IList<Menu> sanPhams =
                               _menuRepository.GetMany(o => o.idControl == 11 &&o.ok &&o.HasValue &&o.HasOnHand&&o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderByDescending(o => o.sDate).ToList();
                ViewData["sx"] = sx.HasValue ? sx.Value : 0;
                SanPhamModel model = new SanPhamModel()
                {
                    Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                    soluongtimduoc = sanPhams.Count(),
                    page = page
                };
                return View("Index", model);
            }
            else
            {
                if (sx.Value == 0)
                {
                    //lọc theo tat ca san pham
                    IList<Menu> sanPhams =
                                 _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderByDescending(o => o.sDate).ToList();
                    SanPhamModel model = new SanPhamModel()
                                                 {
                                                     Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                                                     soluongtimduoc = sanPhams.Count(),
                                                     page = page
                                                 };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 1)
                {
                    //gia tang dan
                    IList<Menu> sanPhams =
                        _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderBy(o => o.PricePro).ToList();
                          
                    SanPhamModel model = new SanPhamModel()
                                                 {
                                                     Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                                                     soluongtimduoc = sanPhams.Count(),
                                                     page = page
                                                 };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 2)
                {
                    //gia giam dan
                    IList<Menu> sanPhams =
                        _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderByDescending(o => o.PricePro).ToList();
                         
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 3)
                {
                    //Id tang dan
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderBy(o => o.id_).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 4)
                {
                    //Id giam dan
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderByDescending(o => o.id_).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 5)
                {
                    //Ten tang dan
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderBy(o => o.NameProduct).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value==6)
                {
                    //Ten giam dan
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan == null && o.MoTaNgan == null).OrderByDescending(o => o.NameProduct).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if(sx.Value==7)
                {
                    //san pham da xong
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan != null && o.MoTaNgan != null).OrderByDescending(o => o.sDateOk).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else
                {
                    //san pham khong du dieu kien hien thi
                    IList<Menu> sanPhams =
                         _menuRepository.GetMany(o => o.idControl == 11 &&o.TenNgan==null &&o.MoTaNgan==null).OrderByDescending(o => o.sDateOk).ToList();
                    SanPhamModel model = new SanPhamModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
            }
        }
        [HttpPost]
        public JsonResult Save(string tenNgan, string motaNgan,int id)
        {
            Menu dlcu = _menuRepository.GetById(id);
            dlcu.TenNgan = tenNgan;
            dlcu.MoTaNgan = motaNgan;
            dlcu.sDateOk = DateTime.Now;
            _menuRepository.Update(dlcu);
            _unitOfWork.Commit();
            return Json(new { mess = "Lưu thành công!", JsonRequestBehavior.AllowGet });
        }
      
        [HttpPost]
        public FileResult ExportExcel(ThemTinTucModel obj)
        {
            string tacvu = obj.Content;
            ShopDataContex db = new ShopDataContex();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[15] 
                                            { new DataColumn("ID"), 
                                            new DataColumn("ID2"), 
                                            new DataColumn("Item title"),
                                            new DataColumn("Final URL"),
                                            new DataColumn("Image URL"),
                                            new DataColumn("Item subtitle"),
                                            new DataColumn("Item description"),
                                            new DataColumn("Item category"),
                                            new DataColumn("Price"),
                                            new DataColumn("Sale price"),
                                            new DataColumn("Contextual keywords"),
                                            new DataColumn("Item address"),
                                            new DataColumn("Tracking template"),
                                            new DataColumn("Custom parameter"),
                                            new DataColumn("Action")
            });

            var Menus = db.Menu.Where(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan != null && o.MoTaNgan != null).OrderByDescending(o => o.sDateOk).ToList();
            foreach (var product in Menus)
            {
                string ID = "BG" + product.id_;
                string ID2 = "";
                string Itemtitle = product.TenNgan;
                string FinalURL = "https://beautygarden.vn/" + product.Link + ".html";
                string ImageURL = "https://beautygarden.vn/Upload/Files/" + product.Img;
                string Itemsubtitle = "beautygarden.vn";
                string Itemdescription = product.MoTaNgan;
                string Itemcategory = "";
                string Price = product.PricePro + " VND";
                string Saleprice = "";
                string Contextualkeywords = "";
                string Itemaddress = "";
                string Trackingtemplate = "";
                string Customparameter = "";
                string Action = tacvu;

                dt.Rows.Add(ID, ID2, Itemtitle,FinalURL,ImageURL,Itemsubtitle,Itemdescription,Itemcategory, Price, Saleprice, Contextualkeywords,
                    Itemaddress,Trackingtemplate, Customparameter,Action);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CSVgoogle.xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult ExportExcelForZalo()
        {
          
            ShopDataContex db = new ShopDataContex();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2]
                                {
                                    new DataColumn("Tên sản phẩm"),
                                    new DataColumn("Link")
                                });
            var Menus = Shop.Web.Model.YKienKhachHangModel.GetDataforZalo();

            //var Menus = db.Menu.Where(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan != null && o.MoTaNgan != null &&o.sDate>= "2017-07-18 17:00:00.000" && o.sDate<= '2017-08-08 08:24:22.222').OrderByDescending(o => o.sDateOk).ToList();
            foreach (var product in Menus)
            {
                string tensp = product.TenNgan;
                string link = "https://beautygarden.vn/" + product.Link + ".html";
                dt.Rows.Add(tensp, link);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelforZalo.xlsx");
                }
            }
        }

        /// <summary>
        /// Export excel remarketing Facebook
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public FileResult ExportExcelRefacebook()
        {
            ShopDataContex db = new ShopDataContex();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10]
                                {
                                    new DataColumn("id"),
                                    new DataColumn("title"),
                                    new DataColumn("description"),
                                    new DataColumn("link"),
                                    new DataColumn("image_link"),
                                    new DataColumn("condition"),                // New
                                    new DataColumn("availability"),             // In Stock
                                    new DataColumn("price"),
                                    new DataColumn("google_product_category"),  //danh muc
                                    new DataColumn("brand")
                                });
            var Menus = Shop.Web.Model.YKienKhachHangModel.GetDataforFacebook();
            string thuonghieu = "";
            foreach (var product in Menus)
            {
                string id = "BGdetail_" + product.id_;
                string tensp = product.NameProductLong;
                string mota = product.MoTaNgan;
                string link = "https://beautygarden.vn/" + product.Link + ".html";
                string ImageURL = "https://beautygarden.vn/Upload/Files/" + product.Img;
                string condition = "New";
                string availability = "In Stock";
                string Price = product.PricePro + " VND";
                string category = Shop.Web.Model.YKienKhachHangModel.GetDanhMucForFacebook(product.id_);
                string brand = Shop.Web.Model.YKienKhachHangModel.GetThuongHieuForFacebook(product.id_);
                if (brand == "Thương Hiệu Khác")
                {
                    thuonghieu = "BeautyGarden";
                }
                else
                {
                    thuonghieu = brand;
                }
                dt.Rows.Add(id,tensp,mota,link, ImageURL, condition, availability,Price,category, thuonghieu);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReFacebook.csv");
                }
            }
        }

        [HttpPost]
        public void ExportClientsListToCSV()
        {

            StringWriter sw = new StringWriter();
            string thuonghieu = "";
            sw.WriteLine("\"id\",\"title\",\"description\",\"link\",\"image_link\",\"condition\",\"availability\",\"price\",\"google_product_category\",\"brand\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ReFacebook.csv");
            Response.ContentType = "text/csv";
            var Menus = Shop.Web.Model.YKienKhachHangModel.GetDataforFacebook();
            foreach (var product in Menus)
            {
                string id = "BGdetail_" + product.id_;
                string tensp = product.NameProductLong;
                string mota = product.MoTaNgan;
                string link = "https://beautygarden.vn/" + product.Link + ".html";
                string ImageURL = "https://beautygarden.vn/Upload/Files/" + product.Img;
                string condition = "New";
                string availability = "In Stock";
                string Price = product.PricePro + " VND";
                string category = Shop.Web.Model.YKienKhachHangModel.GetDanhMucForFacebook(product.id_);
                string brand = Shop.Web.Model.YKienKhachHangModel.GetThuongHieuForFacebook(product.id_);
                if (brand == "Thương Hiệu Khác")
                {
                    thuonghieu = "BeautyGarden";
                }
                else
                {
                    thuonghieu = brand;
                }
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                    id,
                    tensp,
                    mota,
                    link,
                    ImageURL,
                    condition,
                    availability,
                    Price,
                    category,
                    thuonghieu
                    ));
            }
            Response.Write(sw.ToString());
            Response.End();

        }
    }
}
