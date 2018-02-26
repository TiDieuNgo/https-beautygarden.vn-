using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    public class DanhMucSanPhamController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuProAddRepository _menuProAddRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;
        public DanhMucSanPhamController(IMenuRepository menuRepository, IMenuProAddRepository menuProAddRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _menuRepository = menuRepository;
            _menuProAddRepository = menuProAddRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }
        //lay danh muc san pham đổ vào listbox
        public ActionResult Index(int productId)
        {
            IList<DanhMucSelectModel> danhMucs = new List<DanhMucSelectModel>();
            IList<Menu> menus = _menuRepository.GetMany(o => o.Style == "danh-muc-san-pham" && o.idControl != 0).ToList();
            //sua
            if (productId != 0)
            {
                IList<MenuProAdd> menuProAdds = _menuProAddRepository.GetMany(o => o.idMenuProAdded == productId).ToList();
                if (menus.Any())
                {
                    foreach (var menu in menus)
                    {
                        danhMucs.Add(new DanhMucSelectModel()
                        {
                            DanhMucId = menu.id_,
                            Name = menu.NameProduct,
                            Selected = menuProAdds.Any(o => o.idMenuCatelogy == menu.id_)
                        });
                    }
                }

            }
            else
            {
                //them moi
                if (menus.Any())
                {
                    foreach (var menu in menus)
                    {
                        danhMucs.Add(new DanhMucSelectModel()
                                         {
                                             DanhMucId = menu.id_,
                                             Name = menu.NameProduct,
                                             Selected = false
                                         });
                    }
                }
            }


            return View("ListBoxForDanhMuc", danhMucs);
        }

        public JsonResult GetCategoriesTreeview()
        {
            IList<MenuCategoryTree> categoryTrees = _menuRepository.GetCategory();
            return Json(new
            {
                ProductCategories = categoryTrees
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCategoryTree(int productId)
        {
            IList<MenuCategoryTree> categoryTrees = _menuRepository.GetCategory();
            IList<int> menuProAdds = productId != 0 ? _menuProAddRepository.GetMany(o => o.idMenuProAdded == productId).Select(o => o.idMenuCatelogy).ToList() : new List<int>();
            return View("Tree", new CategoryTreeModel
                                    {
                                        ProductId = productId,
                                        Categories = categoryTrees,
                                        SelectedIds = menuProAdds

                                    });
        }
        //hash code để lấy danh sach mã vach
        private List<SelectListItem> DanhSachSanPhamInListBox()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["ShopDataContex"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select NameProduct,id_ from Menu where Menu.Style='danh-muc-san-pham' and idControl<>0";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["NameProduct"].ToString(),
                                Value = sdr["id_"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
        public ActionResult ViewDanhMuc(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Menu> dm =
                _menuRepository.GetMany(o => o.idControl == 1 && o.Style == "danh-muc-san-pham").OrderBy(o=>o.SapxepDanhMuc).ToList();
            var data = dm.ToPagedList(pageNumber, pageSize);
            return View("Index", data);
        }
        //them moi danh muc
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Danh_Muc_Create)))
                return View("_NoAuthor");

            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(ThemDanhMucModel model)
        {
            #region thêm danh mục sản phẩm
            if (ModelState.IsValid)
            {
                Menu menu = new Menu()
                {
                    idControl = 1,//danh muc cha
                    Link = ConvertFont(model.NameProduct),
                    LevelMenu = 1,
                    Img = (model.Img).Replace("/files/", ""),
                    Style = "danh-muc-san-pham",
                    ui = "vi",
                    IdNhaCungCap = 12,
                    Visitor = 0,
                    ok = true,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    idUser = 18,
                    idUserOk = 18,
                    CodeProduct = RejectMarks(model.NameProduct),
                    NameProduct = model.NameProduct,
                    IconMenu = (model.IconMenu).Replace("/files/", ""),
                    ShowMenuHome = model.ShowMenuHome,
                    ShowMenuTop = model.ShowMenuTop,
                    SapxepDanhMuc = model.SapxepDanhMuc,
                    BannerDanhMuc = (model.BannerDanhMuc).Replace("/files/", ""),
                    MoTaDanhMuc = model.MoTaDanhMuc,
                    GiaThiTruong = 0,
                    SEODescription = model.SEODescription,
                    SEOtitle = model.SEOtitle

                };
                _menuRepository.Add(menu);

                _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
                _unitOfWork.Commit();
            }
            else
            {
                ViewData["thongbao"] = "Vui lòng nhập đầy đủ dữ liệu!";
                return View("Create", model);
            }
          
            return RedirectToAction("ViewDanhMuc");

            #endregion
        }
        public ActionResult Delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Danh_Muc_Delete)))
                return View("_NoAuthor");

            var idxoa = _menuRepository.GetById(id);
            _menuRepository.Delete(idxoa);
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc");
        }
        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Danh_Muc_Edit)))
                return View("_NoAuthor");

            var idcu = _menuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Menu model)
        {
            Menu dlcu = _menuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.NameProduct = model.NameProduct;
           
            dlcu.IconMenu = (model.IconMenu).Replace("/files/", "");
            dlcu.Img = (model.Img).Replace("/files/", "");
            dlcu.ShowMenuTop = model.ShowMenuTop;
            dlcu.ShowMenuHome = model.ShowMenuHome;
            dlcu.SapxepDanhMuc = model.SapxepDanhMuc;
            dlcu.SapXepSanPham = model.SapXepSanPham;
            dlcu.BannerDanhMuc = (model.BannerDanhMuc).Replace("/files/", "");
            dlcu.MoTaDanhMuc = model.MoTaDanhMuc;
            dlcu.SEODescription = model.SEODescription;
            dlcu.SEOtitle = model.SEOtitle;
            dlcu.GiaThiTruong = 0;
            _menuRepository.Update(dlcu);
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc");
        }
        public ActionResult CayMenu()
        {
          

            IList<Menu> allCategories =
                _menuRepository.GetMany(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl != 0)
                    .OrderBy(o => o.SapxepDanhMuc)
                    .ToList();

            IList<CayDanhMucItemModel> cayDanhMucItemModels = new List<CayDanhMucItemModel>();
            if (allCategories.Any())
            {
                IList<Menu> list1 = allCategories.Where(o => o.idControl == 1).ToList();
                if (list1.Any())
                {
                    foreach (var l1 in list1)
                    {
                        var ids1 = new List<int>();
                        CayDanhMucItemModel item1 = new CayDanhMucItemModel()
                        {
                            Id = l1.id_,
                            Link = l1.Link,
                            Name = l1.NameProduct,
                        };
                        #region l2
                        IList<Menu> list2 = allCategories.Where(o => o.idControl == l1.id_).ToList();
                        if (list2.Any())
                        {
                            foreach (var l2 in list2)
                            {
                                CayDanhMucItemModel item2 = new CayDanhMucItemModel()
                                {
                                    Id = l2.id_,
                                    Link = l2.Link,
                                    Name = l2.NameProduct,
                                };
                                #region l3
                                IList<Menu> list3 = allCategories.Where(o => o.idControl == l2.id_).ToList();
                                if (list3.Any())
                                {
                                    foreach (var l3 in list3)
                                    {
                                        CayDanhMucItemModel item3 = new CayDanhMucItemModel()
                                        {
                                            Id = l3.id_,
                                            Link = l3.Link,
                                            Name = l3.NameProduct,
                                        };
                                        item3.Q1 = _menuRepository.CountTatCaSanPhamCuaListDanhMucKhongDieuKien(new List<int>() {item3.Id});
                                        item3.Q2 = _menuRepository.CountTatCaSanPhamCuaListDanhMucTheoDieuKien(new List<int>() { item3.Id });

                                        item2.IdsCon.Add(item3.Id);
                                        item2.Childs.Add(item3);
                                    }
                                }
                                #endregion
                                item2.IdsCon.Add(l2.id_);

                                item2.Q1 = _menuRepository.CountTatCaSanPhamCuaListDanhMucKhongDieuKien(item2.IdsCon);
                                item2.Q2 = _menuRepository.CountTatCaSanPhamCuaListDanhMucTheoDieuKien(item2.IdsCon);

                               // item1.IdsCon= item1.IdsCon.Union(item2.IdsCon).ToList();
                                ids1.AddRange(item2.IdsCon);
                                item1.Childs.Add(item2);
                            }
                        }
                        #endregion


                      //  item1.IdsCon.Add(l1.id_);
                        ids1.Add(l1.id_);
                        item1.IdsCon = ids1;

                        item1.Q1 = _menuRepository.CountTatCaSanPhamCuaListDanhMucKhongDieuKien(item1.IdsCon);
                        item1.Q2 = _menuRepository.CountTatCaSanPhamCuaListDanhMucTheoDieuKien(item1.IdsCon);
                        cayDanhMucItemModels.Add(item1);
                    }
                }
            }

            return View("CayMenu", cayDanhMucItemModels);

        }
        public ActionResult CheckDanhMuc(Menu model)
        {
           
            return View("CheckDanhMuc");
        }
        public ActionResult WindowsPopupCayMenu()
        {
            return View("WindowsPopupCayMenu");
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

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}