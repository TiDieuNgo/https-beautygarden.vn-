using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Models;


namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class SanPhamController : BaseController
    {
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        private readonly ITuKhoaTimKiemRepository _tuKhoaTimKiemRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 50;
        private readonly ITagsLinkRepository _tagsLinkRepository;
        private readonly IMenuOptionRepository _menuOptionRepository;
        private readonly IMenuProAddRepository _menuProAddRepository;
        private readonly IMenuImageRepository _menuImageRepository;
        private readonly IThietLapRepository _thietLapRepository;
        private readonly HttpContextBase _httpContext;
        private readonly IKhoQuaTangRepository _khoQuaTangRepository;
        public SanPhamController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, ITagsLinkRepository tagsLinkRepository, IMenuOptionRepository menuOptionRepository, IMenuProAddRepository menuProAddRepository, IMenuImageRepository menuImageRepository, IThietLapRepository thietLapRepository, ITuKhoaTimKiemRepository tuKhoaTimKiemRepository, IDanhSachTagRepository danhSachTagRepository, HttpContextBase httpContext, IKhoQuaTangRepository khoQuaTangRepository)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _tagsLinkRepository = tagsLinkRepository;
            _menuOptionRepository = menuOptionRepository;
            _menuProAddRepository = menuProAddRepository;
            _menuImageRepository = menuImageRepository;
            _thietLapRepository = thietLapRepository;
            _tuKhoaTimKiemRepository = tuKhoaTimKiemRepository;
            _danhSachTagRepository = danhSachTagRepository;
            _httpContext = httpContext;
            _khoQuaTangRepository = khoQuaTangRepository;
        }

        //trang hien thi
        public ActionResult Index(int? sx, int page = 1)
        {
            //if (!CheckRole(_httpContext, int.Parse(Roles.San_Pham_View)))
            //    return View("_NoAuthor");

            int pageNumber = page;
            //lọc theo điều kiện chưa có mã vạch, đã có và sản phẩm hết hàng
            if (!sx.HasValue)//nếu sản phẩm không có giá trị lọc thì vẫn hiển thị bình thường
            {
                IList<Menu> sanPhams =
                               _menuRepository.GetMany(o => o.idControl == 11).OrderByDescending(o => o.sDate).ToList();

                ViewData["sx"] = sx.HasValue ? sx.Value : 0;
                SanPhamViewModel model = new SanPhamViewModel()
                {
                    Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                    soluongtimduoc = sanPhams.Count(),
                    page = page
                };
                return View("Index", model);
            }
            else
            {
                if (sx.Value == 1)
                {
                    //lọc theo sản phẩm chưa có mã vạch
                    IList<Menu> sanPhams =
                              _menuRepository.GetMany(o => o.idControl == 11 && o.HasValue == false).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Chưa có mã vạch";
                    return View("Index", model);
                }
                else if (sx.Value == 2)
                {
                    //lọc theo sản phẩm đã có mã vạch
                    IList<Menu> sanPhams =
                            _menuRepository.GetMany(o => o.idControl == 11 && o.HasValue == true).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Đã có mã vạch";
                    return View("Index", model);
                }
                else if (sx.Value == 3)
                {
                    //lọc theo sản phẩm đã hết hàng
                    IList<Menu> sanPhams =
                            _menuRepository.GetMany(o => o.idControl == 11 && o.HasOnHand == false).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Sản phẩm hết hàng";
                    return View("Index", model);
                }
                else if (sx.Value == 4)
                {
                    //lọc theo sản phẩm mới vừa đăng
                    DateTime ngaydang = DateTime.Parse("04/13/2017");//ngay dang bai dau tien cua du an Son ban giao
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.ok == true && o.sDate >= ngaydang).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", model);
                }
                else if (sx.Value == 6)
                {
                    //lọc theo sản phẩm bi an
                    IList<Menu> sanPhams =
                             _menuRepository.GetMany(o => o.idControl == 11 && o.ok == false).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Sản phẩm hết hàng";
                    return View("Index", model);
                }
                else if (sx.Value == 7)
                {
                    //lọc theo sản phẩm show ra giá tốt mỗi ngày
                    IList<Menu> sanPhams =
                             _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.GiaHot).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Sản phẩm hết hàng";
                    return View("Index", model);
                }
                else if (sx.Value == 8)
                {
                    //lọc theo sản phẩm top 100 ban chay
                    IList<Menu> sanPhams =
                             _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.Bestseller).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Sản phẩm hết hàng";
                    return View("Index", model);
                }
                else
                {
                    //san pham sap xep theo vi tri
                    IList<Menu> sanPhams =
                           _menuRepository.GetMany(o => o.idControl == 11 && o.SapXepSanPham != 1000).OrderByDescending(o => o.sDate).ToList();
                    SanPhamViewModel model = new SanPhamViewModel()
                    {
                        Menus = sanPhams.ToPagedList(pageNumber, pageSize),
                        soluongtimduoc = sanPhams.Count(),
                        page = page
                    };
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    ViewData["barcode"] = "Sản phẩm hết hàng";
                    return View("Index", model);
                }

            }

        }

        //tinh tong san pham hien tai
        public ActionResult thongke()
        {
            IList<Menu> sanPhams =
                             _menuRepository.GetMany(o => o.idControl == 11 && o.ok == true).OrderByDescending(o => o.sDate).ToList();
            ViewData["thongkesanpham"] = sanPhams.Count();
            return View("Thongke");
        }

        //show du lieu ra create
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.San_Pham_Create)))
                return View("_NoAuthor");

            //nha cung cap
            var nhacungcap = _menuRepository.GetMany(o => o.idControl == 10).OrderBy(o => o.sPosition);
            IList<SelectListItem> danhsachNCC = new List<SelectListItem>();
            if (nhacungcap.Any())
            {
                foreach (var ncc in nhacungcap)
                {
                    danhsachNCC.Add(new SelectListItem()
                    {
                        Text = ncc.NameProduct,
                        Value = ncc.id_.ToString()
                    });
                }
            }
            ViewData["lydonenmua"] = _menuRepository.GetLydoNenMua();
            ViewData["nhacungcap"] = danhsachNCC;
            return View("Create", new Menu());
        }

        public ActionResult GetImages(int productId)
        {
            IList<MenuImageMapping.MenuImage> images = new List<MenuImageMapping.MenuImage>();
            if (productId != 0)
            {
                images = _menuImageRepository.GetImagesByMenuId(productId);
            }

            return View("Images", images);
        }
        //sua du lieu
        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.San_Pham_Edit)))
                return View("_NoAuthor");

            //(1)danh muc san pham
            var dlcu = _menuRepository.GetById(id);
            var danhmucs = _menuRepository.GetMany(o => o.Style == "danh-muc-san-pham");
            IList<SelectListItem> listItems = new List<SelectListItem>();

            if (danhmucs.Any())
            {
                foreach (var danhmuc in danhmucs)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = danhmuc.NameProduct,
                        Value = danhmuc.id_.ToString()
                    });
                }
            }
            ViewData["danhSachDanhMuc"] = listItems;
            //(2) danh sach tag

            //(3)nha cung cap
            var nhacungcap = _menuRepository.GetMany(o => o.idControl == 10).OrderBy(o => o.sPosition);
            IList<SelectListItem> danhsachNCC = new List<SelectListItem>();

            if (nhacungcap.Any())
            {
                foreach (var danhmuc in nhacungcap)
                {
                    danhsachNCC.Add(new SelectListItem()
                    {
                        Text = danhmuc.NameProduct,
                        Value = danhmuc.id_.ToString()
                    });
                }
            }
            ViewData["nhacungcap"] = danhsachNCC;
            //dua vao id de lay du lieu xem tiêu đề nội dung nào không có dữ liệu thì ẩn đi
            ViewData["viewnoidung1"] = _menuRepository.CheckContent1(id);
            ViewData["viewnoidung2"] = _menuRepository.CheckContent2(id);
            ViewData["viewnoidung3"] = _menuRepository.CheckContent3(id);
            ViewData["viewnoidung4"] = _menuRepository.CheckContent4(id);
            ViewData["viewnoidung5"] = _menuRepository.CheckContent5(id);
            return View("Edit", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult DeleteSmallImage(int Id)
        {
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand("delete from MenuImage where id={0}", Id);
            }
            return Json(new
            {

                message = "xoa thành công!",
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(SaveProductFormModel model)
        {
            #region san pham
            //SaveProductFormModel
            Menu dlcu = _menuRepository.GetById(model.id_);
            dlcu.NameProduct = model.NameProduct;
            dlcu.Img = (model.Img).Replace("/files/", "");
            dlcu.IdNhaCungCap = model.IdNhaCungCap;
            dlcu.id_ = model.id_;
            dlcu.Option1 = model.Option1;
            dlcu.Option5 = model.Option5;
            dlcu.Option6 = model.Option6;
            dlcu.ContentLabel = model.ContentLabel;
            dlcu.ContentLabel1 = model.ContentLabel1;
            dlcu.ContentLabel2 = model.ContentLabel2;
            dlcu.ContentLabel3 = model.ContentLabel3;
            dlcu.ContentLabel4 = model.ContentLabel4;
            //dlcu.Content = ChangeImageSEO(model.Content, model.NameProduct, ConvertFont(model.NameProductLong));
            //dlcu.Content1 = ChangeImageSEO1(model.Content1, model.NameProduct, ConvertFont(model.NameProductLong));
            //dlcu.Content2 = ChangeImageSEO2(model.Content2, model.NameProduct, ConvertFont(model.NameProductLong));
            //dlcu.Content3 = ChangeImageSEO3(model.Content3, model.NameProduct, ConvertFont(model.NameProductLong));
            //dlcu.Content4 = ChangeImageSEO4(model.Content4, model.NameProduct, ConvertFont(model.NameProductLong));
            dlcu.Content = model.Content;
            dlcu.Content1 = model.Content1;
            dlcu.Content2 = model.Content2;
            dlcu.Content3 = model.Content3;
            dlcu.Content4 = model.Content4;
            dlcu.Note = model.Note;
            dlcu.SapXepSanPham = model.SapXepSanPham;
            dlcu.sDate = DateTime.Now;
            dlcu.sDateOk = DateTime.Now;
            dlcu.ContentLabelTaiSao = model.ContentLabelTaiSao;
            dlcu.ContentTaiSao = model.ContentTaiSao;
            dlcu.BarCode = model.BarCode;
            dlcu.ContentTheoSp = model.ContentTheoSp;
            dlcu.NameProductLong = model.NameProductLong;
            dlcu.Link = model.Link; //sửa sản phẩm không sửa tên thành link nữa, cho sửa link trực tiếp
            dlcu.DungSai = true;
            dlcu.SEOtitle = model.SEOtitle;
            dlcu.SEODescription = model.SEODescription;
            dlcu.NguoiTao = User.Identity.Name;
            dlcu.Bestseller = false;
            //dlcu.NgayHetHang = DateTime.Now;
            #endregion san pham
            #region hinh anh khac
            //------------- hinh anh khac -----------------


            IList<MenuImageMapping.MenuImage> images = !string.IsNullOrEmpty(model.OtherImages)
                                                       ? JsonConvert.DeserializeObject
                                                             <IList<MenuImageMapping.MenuImage>>(model.OtherImages)
                                                       : new List<MenuImageMapping.MenuImage>();
            foreach (var menuImage in images)
            {
                if (menuImage.id == 0)//them moi hinh anh
                {
                    MenuImage img = new MenuImage()
                    {
                        idMenu = model.id_,
                        date = DateTime.Now,
                        ImageName = menuImage.ImageName,
                    };
                    _menuImageRepository.Add(img);

                }
            }
            #endregion hinh anh khac
            #region danh muc

            IList<int> idDanhMucs = !string.IsNullOrEmpty(model.DanhMucIds)
                                        ? JsonConvert.DeserializeObject
                                              <IList<int>>(model.DanhMucIds)
                                        : new List<int>();
            if (model.id_ != 0)
            {
                IList<MenuProAdd> menusDanhMuc = _menuProAddRepository.GetMany(o => o.idMenuProAdded == model.id_).ToList();
                foreach (var menuProAdd in menusDanhMuc)
                {
                    if (!idDanhMucs.Contains(menuProAdd.idMenuCatelogy))
                    {
                        //delete nhung thang khong co trong danh sach idDanhMucs
                        _menuProAddRepository.Delete(menuProAdd);
                        _unitOfWork.Commit();
                    }
                }
                if (idDanhMucs.Any())
                {
                    //lay danh sach menuproadd hien tai cua product

                    foreach (var idDanhMuc in idDanhMucs)
                    {
                        if (!menusDanhMuc.Any(o => o.idMenuCatelogy == idDanhMuc))
                        {
                            MenuProAdd menuProAdd = new MenuProAdd()
                            {
                                idMenuCatelogy = idDanhMuc,
                                idMenuProAdded = model.id_,
                                sDate = DateTime.Now,
                                sDateOk = DateTime.Now,
                                Style = "add-san-pham",
                                idUser = 15,
                                idUserOk = 15

                            };
                            _menuProAddRepository.Add(menuProAdd);
                            _unitOfWork.Commit();
                        }


                    }
                }
            

            }
            else
            {
                if (idDanhMucs.Any())
                {
                    foreach (var idDanhMuc in idDanhMucs)
                    {
                        MenuProAdd menuProAdd = new MenuProAdd()
                        {
                            idMenuCatelogy = idDanhMuc,
                            idMenuProAdded = model.id_,
                            sDate = DateTime.Now,
                            sDateOk = DateTime.Now,
                            Style = "add-san-pham",
                            idUser = 15,
                            idUserOk = 15


                        };
                        _menuProAddRepository.Add(menuProAdd);
                        _unitOfWork.Commit();
                    }
                }
            }

            #endregion
            #region danh sach tag
            string[] separators = { "," };
            var taglist = model.mySingleField.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //check null: nếu tồn tại thêm mới bình thường
            if (taglist.Any())
            {
                foreach (var tag in taglist)
                {
                    //check trung
                    int checktrung = _danhSachTagRepository.GetTagNamebyIdmenu(model.id_, tag.Trim());
                    if (checktrung == 0)
                    {
                        DanhSachTag _tag = new DanhSachTag()
                        {
                            NgayTao = DateTime.Now,
                            IdMenu = model.id_,
                            NguoiTao = 18,
                            TenTag = tag.Trim(),
                            Code = RejectMarks(tag.Trim())
                        };
                        _danhSachTagRepository.Add(_tag);
                    }
                }
            }
            #endregion
            #region update kho quà tặng

            var idkhoqt = _khoQuaTangRepository.Get(o => o.IdMenu == model.id_);
            if (idkhoqt == null)
            {
                //thêm mới
                #region thêm mới kho quà tặng

                if (model.IdQUaTangs != null)
                {
                    KhoQuaTang khoQuaTang = new KhoQuaTang()
                                            {
                                                IdMenu = model.id_,
                                                NgayTao = DateTime.Now,
                                                IdSanPhamTang = model.IdQUaTangs.Remove(0, 1)
                                            };
                    _khoQuaTangRepository.Add(khoQuaTang);
                    _unitOfWork.Commit();
                }
              
                #endregion
            }
            else
            {
                //update
                KhoQuaTang kho = _khoQuaTangRepository.GetById(idkhoqt.Id);
                kho.Id = idkhoqt.Id;
                kho.IdMenu = model.id_;
                kho.NgayTao = DateTime.Now;
                kho.IdSanPhamTang = model.IdQUaTangs;
                _khoQuaTangRepository.Update(kho);
            }

            #endregion
            _menuRepository.Update(dlcu);

            _menuRepository.ClearCacheByKey(new List<string>(){Shop.Web.Core.Cache.CacheKey.AllMenu});
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        //xoa
        public ActionResult Delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.San_Pham_Delete)))
                return View("_NoAuthor");

            var xoa = _menuRepository.GetById(id);
            _menuRepository.Delete(xoa);
            _unitOfWork.Commit();
            //sau khi xoa sản phẩm xong xóa luôn mã vạch đi kèm
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand("delete from MenuOption where MenuOption.IdMenu={0}", id);
            }
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            return RedirectToAction("Index");
        }
        //tim kiem san pham
        public ActionResult TimKiem(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            //keyword = RejectMarks(keyword);

            IList<Menu> sanPhams = _menuRepository.TimKiemByTenAndBarcode(keyword.Trim().ToLower());

            SanPhamViewModel timkiem = new SanPhamViewModel()
            {
                Menus = sanPhams.ToPagedList(pageNumber, pageSize),

                soluongtimduoc = sanPhams.Count(),
            };
            ViewData["key"] = keyword;
            return View(timkiem);
        }

        /// <summary>
        /// Luu san pham
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Save(SaveProductFormModel model)
        {
            #region thêm sản phẩm
            Menu menu = new Menu()
                          {
                              CodeProduct = RejectMarks(model.NameProductLong),
                              PriceOffPro = model.PriceOffPro,
                            Content = ChangeImageSEO(model.Content, model.NameProduct, ConvertFont(model.NameProductLong)),
                            Content1 = ChangeImageSEO1(model.Content1, model.NameProduct, ConvertFont(model.NameProductLong)),
                            Content2 = ChangeImageSEO2(model.Content2, model.NameProduct, ConvertFont(model.NameProductLong)),
                            Content3 = ChangeImageSEO3(model.Content3, model.NameProduct, ConvertFont(model.NameProductLong)),
                            Content4 = ChangeImageSEO4(model.Content4, model.NameProduct, ConvertFont(model.NameProductLong)),
                            //Content = model.Content,
                            //Content1 = model.Content1,
                            //Content2 = model.Content2,
                            //Content3 = model.Content3,
                            //Content4 = model.Content4,
                            Img = (model.Img).Replace("/files/", ""),
                              IdNhaCungCap = model.IdNhaCungCap,
                              idControl = 11,
                              Note = model.Note,
                              ok = false,//vua them thi san pham an di
                              Option1 = model.Option1,
                              Option5 = model.Option5,
                              Option6 = model.Option6,
                              Menu2 = "",
                              MenuAdwords = "",
                              Link = ConvertFont(model.NameProductLong),
                              LevelMenu = 1,
                              LinkHttp = "https://beautygarden.vn/" + ConvertFont(model.NameProduct) + ".html",
                              LinkHttp1 = "",
                              Style = "san-pham",
                              ui = "vi",
                              ContentLabel = model.ContentLabel,
                              Option = true,
                              ContentLabel1 = model.ContentLabel1,
                              ContentLabel2 = model.ContentLabel2,
                              ContentLabel3 = model.ContentLabel3,
                              ContentLabel4 = model.ContentLabel4,
                              VIP = false,
                              Option8 = true,
                              Option9 = model.Option9,
                              sPosition = model.sPosition,
                              Visitor = model.Visitor,
                              sDate = DateTime.Now,
                              sDateOk = DateTime.Now,
                              idUser = 18,
                              idUserOk = 18,
                              Option2 = true,
                              Option3 = true,
                              Option4 = true,
                              SEOKeyWord = "",
                              NumberHaveGift = 0,
                              idMPADSys = 0,
                              HasSale = model.HasSale,
                              HasValue = false,
                              NameProduct = model.NameProduct,
                              BarCode = string.IsNullOrEmpty(model.BarCode) ? "" : model.BarCode.Trim(),//luu tam thoi ben bang menu sau do lay du lieu do qua bang menuoption
                              HasOnHand = true,//mac dinh =1 tuc la con hang. sau khi kiem tra ma vach thi update lai onhand
                              SapXepSanPham = model.SapXepSanPham,
                              SapxepDanhMuc = model.SapxepDanhMuc,
                              BarcodeType = model.BarcodeType,
                              ContentTaiSao = model.ContentTaiSao,
                              ContentLabelTaiSao = model.ContentLabelTaiSao,
                              ContentTheoSp = model.ContentTheoSp,
                              GiaHot = false,
                              NameProductLong = model.NameProductLong,
                              DungSai = true,
                              SEODescription = model.SEODescription,
                              SEOtitle = model.SEOtitle,
                              NgayHetHang = DateTime.Now,
                              NguoiTao = User.Identity.Name,
                              Bestseller = false
                          };
           
            _menuRepository.Add(menu);
            _unitOfWork.Commit();
            //sau khi thêm mới sp xong insert 4 sao cho phần đánh giá
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand("INSERT INTO UserRatings (Rating, IdSanPham)VALUES (4, {0})", menu.id_);
            }
            #endregion
            #region hinh anh khac
            //luu hinh anh khac
            IList<MenuImageMapping.MenuImage> images = !string.IsNullOrEmpty(model.OtherImages)
                                                         ? JsonConvert.DeserializeObject
                                                               <IList<MenuImageMapping.MenuImage>>(model.OtherImages)
                                                         : new List<MenuImageMapping.MenuImage>();
            if (images.Any())
            {
                foreach (var menuImage in images)
                {
                    if (menuImage.id == 0)//them moi hinh anh
                    {
                        MenuImage img = new MenuImage()
                        {
                            idMenu = menu.id_,
                            date = DateTime.Now,
                            ImageName = menuImage.ImageName,
                        };
                        _menuImageRepository.Add(img);
                        _unitOfWork.Commit();
                    }

                }
            }
            #endregion
            #region danh muc

            IList<int> idDanhMucs = !string.IsNullOrEmpty(model.DanhMucIds)
                                        ? JsonConvert.DeserializeObject
                                              <IList<int>>(model.DanhMucIds)
                                        : new List<int>();
            if (model.id_ != 0)
            {
                IList<MenuProAdd> menusDanhMuc = _menuProAddRepository.GetMany(o => o.idMenuProAdded == model.id_).ToList();
                foreach (var menuProAdd in menusDanhMuc)
                {
                    if (!idDanhMucs.Contains(menuProAdd.idMenuCatelogy))
                    {
                        //delete nhung thang khong co trong danh sach idDanhMucs
                        _menuProAddRepository.Delete(menuProAdd);
                        _unitOfWork.Commit();
                    }
                }
                if (menusDanhMuc.Any())
                {
                    if (idDanhMucs.Any())
                    {
                        //lay danh sach menuproadd hien tai cua product

                        foreach (var idDanhMuc in idDanhMucs)
                        {
                            if (!menusDanhMuc.Any(o => o.idMenuCatelogy == idDanhMuc))
                            {
                                MenuProAdd menuProAdd = new MenuProAdd()
                                                          {
                                                              idMenuCatelogy = idDanhMuc,
                                                              idMenuProAdded = menu.id_,
                                                              sDate = DateTime.Now,
                                                              sDateOk = DateTime.Now,
                                                              Style = "add-san-pham",
                                                              idUser = 15,
                                                              idUserOk = 15

                                                          };
                                _menuProAddRepository.Add(menuProAdd);
                                _unitOfWork.Commit();
                            }


                        }
                    }

                    IList<int> removeMenuProdIds =
                        menusDanhMuc.Where(o => !idDanhMucs.Contains(o.idMenuCatelogy)).Select(o => o.id_).ToList();
                    if (removeMenuProdIds.Any())
                        _menuProAddRepository.Delete(o => removeMenuProdIds.Contains(o.id_));
                }

            }
            else
            {
                if (idDanhMucs.Any())
                {
                    foreach (var idDanhMuc in idDanhMucs)
                    {
                        MenuProAdd menuProAdd = new MenuProAdd()
                        {
                            idMenuCatelogy = idDanhMuc,
                            idMenuProAdded = menu.id_,
                            sDate = DateTime.Now,
                            sDateOk = DateTime.Now,
                            Style = "add-san-pham",
                            idUser = 15,
                            idUserOk = 15


                        };
                        _menuProAddRepository.Add(menuProAdd);
                        _unitOfWork.Commit();
                    }
                }
            }

            #endregion
            #region ma vach

            IList<MenuOptionMapping.OptionShow> maVachs = !string.IsNullOrEmpty(model.MaVachJson)
                                                      ? JsonConvert.DeserializeObject
                                                            <IList<MenuOptionMapping.OptionShow>>(model.MaVachJson)
                                                      : new List<MenuOptionMapping.OptionShow>();
            IList<MenuOption> menuOptionsCu = _menuOptionRepository.GetMany(o => o.IdMenu == model.id_).ToList();
            if (maVachs.Any())
            {
                foreach (var optionShow in maVachs)
                {
                    if (optionShow.id_ == 0)//them moi ma vach
                    {
                        MenuOption menuOption = new MenuOption()
                        {
                            IdMenu = menu.id_,
                            TenLoai = optionShow.TenLoai,
                            Img = optionShow.Img.Replace("/files/", ""),
                            Barcode = string.IsNullOrEmpty(optionShow.Barcode) ? "" : optionShow.Barcode.Trim(),
                            Flag = optionShow.Flag,
                            NameProduct = menu.NameProduct,
                            SDate = DateTime.Now,
                            sDateOk = DateTime.Now
                        };
                        //truoc khi luu ma vach kiem tra ma vach co ton tai trong he thong hay khong
                        int istontai = _menuRepository.ChekBarcode(string.IsNullOrEmpty(optionShow.Barcode) ? "" : optionShow.Barcode.Trim());
                        if (istontai > 0)//ma vach ton tai ==> them duoc
                        {
                            _menuOptionRepository.Add(menuOption);
                            _unitOfWork.Commit();
                            //update hasvalue=1
                            using (var context = new ShopDataContex())
                            {
                                context.Database.ExecuteSqlCommand("update Menu set HasValue='True' where id_ ={0}", menu.id_);
                                //update barcodetype
                                context.Database.ExecuteSqlCommand("update Menu set BarcodeType={0} where id_ ={1}", optionShow.Flag, menu.id_);

                            }
                            //them dl thanh cong roi thi kiem tra san pham: onhand=0 thi an di nguoc lai show ra
                            //neu tong onhand <=0  het hang. nguoc lai con hang
                            int isonhand = _menuRepository.CheckOnhand(optionShow.Barcode.Trim());
                            if (isonhand <= 0)
                            {
                                //update hasOnhand =0

                                using (var context = new ShopDataContex())
                                {
                                    context.Database.ExecuteSqlCommand("update Menu set HasOnHand='false' where id_ ={0}", menu.id_);

                                }
                            }

                        }
                        else
                        {

                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        MenuOption sua = menuOptionsCu.FirstOrDefault(o => o.id_ == optionShow.id_);//sua ma vach
                        if (sua != null)
                        {
                            sua.Img = optionShow.Img;
                            sua.Barcode = optionShow.Barcode.Trim();
                            sua.TenLoai = optionShow.TenLoai;
                            sua.Flag = optionShow.Flag;
                            _menuOptionRepository.Update(sua);
                            _unitOfWork.Commit();
                        }
                    }

                }
            }


            #endregion
            #region ma vach khong co gi
            //cach 1:lay barcode cua bang menu sau do luu vao bang meuoption
            //truoc khi luu ma vach kiem tra ma vach co ton tai trong he thong hay khong
            if (model.BarcodeType == 0)
            {
                int somavach = _menuRepository.ChekBarcode(menu.BarCode.Trim());
                if (somavach > 0)//ma vach ton tai ==> them duoc
                {
                    //luu san pham
                    MenuOption menuOp = new MenuOption()
                    {
                        IdMenu = menu.id_,
                        TenLoai = "không có gì",
                        Img = "images.png",
                        Barcode = menu.BarCode,
                        SDate = DateTime.Now,
                        sDateOk = DateTime.Now,
                        NameProduct = menu.NameProduct,
                        Flag = (short)BarcodeType.KhongCoGi
                    };
                    //luu ma vach
                    _menuOptionRepository.Add(menuOp);
                    _unitOfWork.Commit();

                    //update hasvalue=1
                    // _menuRepository.UpdateHasvalue(menu.id_, true);
                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("update Menu set HasValue='True' where id_ ={0}", menu.id_);
                        //update barcodetype=0 neu la san pham khong co gi
                        context.Database.ExecuteSqlCommand("update Menu set BarcodeType=0 where id_ ={0}", menu.id_);

                    }
                    //them dl thanh cong roi thi kiem tra san pham: onhand=0 thi an di nguoc lai show ra
                    //neu tong onhand <=0  het hang. nguoc lai con hang
                    int tonkho = _menuRepository.CheckOnhand(menu.BarCode);
                    if (tonkho <= 0 )
                    {
                        //update hasOnhand = 0
                        using (var context = new ShopDataContex())
                        {
                            context.Database.ExecuteSqlCommand("update Menu set HasOnHand='false' where id_ ={0}", menu.id_);
                        }
                    }
                    //  kiểm tra nếu mã vạch là CB hoặc cb tức là (Combo) thì update Hasonhand =1
                    if (menu.BarCode.IndexOf("CB") != -1 || menu.BarCode.IndexOf("cb") != -1)
                    {
                        using (var context = new ShopDataContex())
                        {
                            context.Database.ExecuteSqlCommand("update Menu set HasOnHand='True' where id_ ={0}", menu.id_);
                        }
                    }
                }
            }

            #endregion
            #region thêm mới tag

            string[] separators = { "," };
            var taglist = model.mySingleField.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //check null: nếu tồn tại thêm mới bình thường
            if (taglist.Any())
            {
                foreach (var tag in taglist)
                {
                    DanhSachTag _tag = new DanhSachTag()
                    {
                        NgayTao = DateTime.Now,
                        IdMenu = menu.id_,
                        NguoiTao = 18,
                        TenTag = tag.Trim(),
                        Code = RejectMarks(tag.Trim())
                    };
                    _danhSachTagRepository.Add(_tag);
                    _unitOfWork.Commit();
                }
            }
            #endregion
            #region thêm mới kho quà tặng

            if (model.IdQUaTangs !=null)
            {
                KhoQuaTang khoQuaTang = new KhoQuaTang()
                                        {
                                            IdMenu = menu.id_,
                                            NgayTao = DateTime.Now,
                                            IdSanPhamTang = model.IdQUaTangs
                                        };
                _khoQuaTangRepository.Add(khoQuaTang);
                _unitOfWork.Commit();
            }
          
            #endregion

            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });

            return RedirectToAction("Index");

        }

        public ActionResult EditMaVach(int id)
        {

            ViewData["TenSp"] = _menuRepository.GetTenSanPham(id);
            int slmavach = _menuOptionRepository.CountMaVachByIdSanPham(id);
            if (slmavach != 0)//co ma vach
            {
                EditMaVachWinDowsOpen model = new EditMaVachWinDowsOpen()
                {
                    DanhSachMaVachs = _menuOptionRepository.GetBarcode(id),
                    //kiem tra san pham do co ma vach hay chua
                    //1. neu co roi thi load len: khong co gi, mau, mui
                    //2. neu chua co thi mac dinh radio button
                    CountMaVach = _menuOptionRepository.CountMaVachByIdSanPham(id),
                    //lay flag cua dung san pham dang load ra
                    Flag = _menuOptionRepository.CheckMauOrMui(id),
                };
                return View("ColorPopupEditMaVach", model);//thay doi PopupEditMaVach
            }
            else
            {

                EditMaVachWinDowsOpen model = new EditMaVachWinDowsOpen()
                {
                    DanhSachMaVachs = _menuOptionRepository.GetBarcode(id),
                    //kiem tra san pham do co ma vach hay chua
                    //1. neu co roi thi load len: khong co gi, mau, mui
                    //2. neu chua co thi mac dinh radio button
                    CountMaVach = _menuOptionRepository.CountMaVachByIdSanPham(id),
                    //lay flag cua dung san pham dang load ra

                };
                return View("ColorPopupEditMaVach", model);
            }


        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditPopupMaVach(BarcodeSubmitModel model)
        {

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult UpdateOk(int idsanpham, bool Checked)
        {

            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='False' where id_ ={0}", idsanpham);

                }
            }
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateTop100(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set Bestseller='True' where id_ ={0}", idsanpham);
                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set Bestseller='False' where id_ ={0}", idsanpham);
                }
            }
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult DeleteBarcode(int id, int IdMenu)
        {
            //khi xoa ma vach chia ra 2 TH
            //1. neu ma vach do co 1 dong: tuc la khong co gi thi cap nhat lai trang thai ben bang Menu
            //2. neu ma vach co nhieu dong: tuc la mau hoac mui thi khi nao xoa het tat ca cac dong moi cap nhat trang thai hasvalue ben bang Menu
            var idxoa = _menuOptionRepository.GetById(id);
            //kiem tra san pham do co bao nhieu dong
            int sodongtrongSP = _menuOptionRepository.LaySoLuongBarcodeInMenuOption(IdMenu);
            _menuOptionRepository.Delete(idxoa);
            _unitOfWork.Commit();
            if (sodongtrongSP == 1) //tuc la ma vach khong co gi
            {
                //update san pham do thanh san pham chua co ma vach
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set HasValue='False' where id_ ={0}", IdMenu);
                }
            }
            return Json(new
                            {
                                ok = true,
                                message = "Xóa mã vạch thành công!"

                            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProductsByCategoryIds(string ids)
        {
            IList<MenuCategoryForAddPromotionmapping> products = new List<MenuCategoryForAddPromotionmapping>();
            if (!string.IsNullOrEmpty(ids))
            {
                IList<int> categoryIds = ids.Split(',').Select(o => Convert.ToInt32(o)).ToList();
                if (categoryIds.Any())
                {
                    products = _menuRepository.GetListProductsByCategoryIds(categoryIds);
                }
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductsByIds(SanPhamIdsModel mpodel)
        {
            IList<MenuCategoryForAddPromotionmapping> products = new List<MenuCategoryForAddPromotionmapping>();

            if (!string.IsNullOrEmpty(mpodel.ids))
            {
                IList<int> productIds = mpodel.ids.Split(',').Select(o => Convert.ToInt32(o)).ToList();
                if (productIds.Any())
                {
                    products = _menuRepository.GetByIds(productIds);
                }
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllProducts()
        {
            IList<MenuCategoryForAddPromotionmapping> products = _menuRepository.GetAllProductsForPromotion();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductAutocomplete(string q)
        {
            IList<MenuCategoryForAddPromotionmapping> products = new List<MenuCategoryForAddPromotionmapping>();
            if (!string.IsNullOrEmpty(q))
                products = _menuRepository.AutocompleteSearch(q);
            return Json(products, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LydoCreate()
        {
            ThietLap thietLap = _thietLapRepository.GetAll().FirstOrDefault();
            return View("LydoCreate", thietLap);
        }

        public static int RandomGiaThiTruong(int giatien, int giathitruong)
        {
            if (giathitruong != 0)
            {
                Random rnd = new Random();
                int phantram = rnd.Next(10, 20);
                giathitruong = giatien + (giatien * (phantram / 100));
            }
            return giathitruong;
        }

        [HttpPost]
        public JsonResult UpdateGiaHot(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set GiaHot='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set GiaHot='False' where id_ ={0}", idsanpham);

                }
            }
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }

        //lay danh sach tag có sẵn để chọn cho nhanh
        [HttpGet]
        public JsonResult getListTag()
        {
            IList<TagName> _Tag = _danhSachTagRepository.GetTagName();
            return Json(_Tag, JsonRequestBehavior.AllowGet);
        }

        //load tag khi edit
        public JsonResult setValEdit(int IdMenu)
        {
            IList<TagName> _tagedit = _danhSachTagRepository.GetTagEdit(IdMenu);
            return Json(_tagedit, JsonRequestBehavior.AllowGet);
        }
      
    }

}
