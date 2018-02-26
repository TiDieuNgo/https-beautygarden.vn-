using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class KhoQuaTangController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKhoQuaTangRepository _khoQuaTangRepository;
        private readonly IMenuRepository _menuRepository;

        public KhoQuaTangController(IUnitOfWork unitOfWork, IKhoQuaTangRepository khoQuaTangRepository, IMenuRepository menuRepository)
        {
            _unitOfWork = unitOfWork;
            _khoQuaTangRepository = khoQuaTangRepository;
            _menuRepository = menuRepository;
        }

        public ActionResult Index()
        {
            var dl = _khoQuaTangRepository.GetAll();
            return View(dl);
        }

        //  kho qua tang
        public ActionResult Add()
        {
            var sanphamlist = new List<SelectListItem>() { new SelectListItem() { Text = "---chọn sản phẩm---", Value = "0" } };
            IList<Menu> sanPhams = _menuRepository.GetMany(o => o.ok && o.HasOnHand && o.HasValue)
                .OrderByDescending(o => o.sDate).ToList();
            sanphamlist.AddRange(from o in sanPhams
                                 where o.idControl == 11 && o.HasValue && o.HasOnHand && o.ok
                                 select new SelectListItem()
                                 { Text = o.NameProduct, Value = o.id_.ToString() });

            return View("Create", new SaveProductFormModel()
            {
                SanPhams = sanphamlist
            });
        }

        public ActionResult Edit(int id) //idsanpham
        {
            IList<ProductGiftSelect>models=new List<ProductGiftSelect>();
            //tu id san pham xún db khoquatangs lay dc idsanpham qua tang va load len
            var layDlcu = _khoQuaTangRepository.Get(o=>o.IdMenu==id);
            IList<int>idsDaChon=new List<int>();
            if (layDlcu !=  null && !string.IsNullOrEmpty(layDlcu.IdSanPhamTang))
            {
                idsDaChon = layDlcu.IdSanPhamTang.Split(',').Select(o => Convert.ToInt32(o)).ToList();
            }
            //lay tat ca sp
            IList<Menu> menus = _menuRepository.GetMany(o => o.idControl == 11 && o.ok && o.HasOnHand && o.HasValue)
                .ToList();
            foreach (var menu in menus)
            {
                models.Add(new ProductGiftSelect()
                           {
                               Name = menu.NameProduct,
                               ProductId = menu.id_,
                               Selected = idsDaChon.Contains(menu.id_)
                });
            }
            return View("Edit", models);
        }
    }
}
