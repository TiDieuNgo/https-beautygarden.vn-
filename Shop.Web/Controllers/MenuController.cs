using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Models;


namespace Shop.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDanhMucTienIchRepository _danhMucTienIchRepository;
        private readonly IPromotionRepository _promotionRepository;
        public MenuController(IMenuRepository menuRepository, IDanhMucTienIchRepository danhMucTienIchRepository, IPromotionRepository promotionRepository)
        {
            _menuRepository = menuRepository;
            _danhMucTienIchRepository = danhMucTienIchRepository;
            _promotionRepository = promotionRepository;
        }

        public ActionResult MenuTop()
        {
            var promotions = _promotionRepository.GetActives();
            var allMenus = _menuRepository.GetAllMenuCache();

            DanhMucViewModel model = new DanhMucViewModel()
            {
                DanhMucs = allMenus.Where(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl != 0).ToList(),
                HasPromotion = promotions.Any()
            };
            return View("MenuTop", model);
        }
        public ActionResult DanhMucConLevel2()
        {
            IList<Menu> boxsanpham =
              _menuRepository.GetMany(o => o.ok && o.Style == "danh-muc-san-pham" && o.idControl == 1).OrderBy(o => o.SapxepDanhMuc).ToList();
            foreach (var danhmuccon in boxsanpham)
            {
                IList<Menu> dmcon = _menuRepository.GetDanhMucConCuaMenuTop(danhmuccon.id_);
            }

            return View("DanhMucInBoxSP");
        }
        public ActionResult DanhMucInDanhSachSp(int? id)
        {
            IList<Menu> danhMucs =
                _menuRepository.GetMany(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl != 0).ToList();

            IList<DanhMucTree> danhMucTrees = new List<DanhMucTree>();
            if (danhMucs.Any())
            {
                IList<Menu> list1 =
                    danhMucs.Where(o => o.Style == "danh-muc-san-pham" && o.idControl == 1 && o.ok == true).ToList();
                foreach (var danhMuc in list1)
                {
                    IList<DanhMucTree> danhmuccap2 =
                        danhMucs.Where(o => o.idControl == danhMuc.id_).Select(o => new DanhMucTree
                        {
                            Id = o.id_,
                            Name = o.NameProduct,
                            Link = o.Link,
                            Selected = id.HasValue && o.id_ == id.Value
                        }).ToList();
                    IList<int> ids = danhmuccap2.Any() ? danhmuccap2.Select(o => o.Id).ToList() : new List<int>();

                    DanhMucTree danhMucTree = new DanhMucTree()
                    {
                        Id = danhMuc.id_,
                        Name = danhMuc.NameProduct,
                        Link = danhMuc.Link,
                        Selected = id.HasValue && (danhMuc.id_ == id.Value || ids.Contains(id.Value)),
                        Childs = danhmuccap2
                    };

                    danhMucTrees.Add(danhMucTree);
                }
            }
            return View("DanhMucInDanhSachSP", danhMucTrees);
        }
        public ActionResult Catagories()
        {
            DanhMucViewModel model = new DanhMucViewModel()
            {
                DanhMucs = _menuRepository.GetMany(o => o.ok == true && o.Style == "danh-muc-san-pham" && o.idControl != 0).ToList()
            };
            return View("MenuPartial", model);
        }
        public ActionResult DanhMucTienIch()
        {
            DanhMucTienIchViewModel model = new DanhMucTienIchViewModel()
            {
                DanhMucs = _danhMucTienIchRepository.GetAll().ToList()
            };
            return View("DanhMucTienIch", model);
        }
        public ActionResult DanhMucTienIchTinh()
        {
            return View("DanhMucTienIchTinh");
        }
    }
}
