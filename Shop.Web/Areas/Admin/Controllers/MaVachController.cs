using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
      [ShopAuthorize]
    public class MaVachController : Controller
    {
        private int countTong = 0;
        private readonly IMenuOptionRepository _menuOptionRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IProductStockSyncRepository _productStockSyncRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MaVachController(IMenuOptionRepository menuOptionRepository, IUnitOfWork unitOfWork,
                                IMenuRepository menuRepository, IProductStockSyncRepository productStockSyncRepository)
        {
            _menuOptionRepository = menuOptionRepository;
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
            _productStockSyncRepository = productStockSyncRepository;
        }

        private const int pageSize = 20;

        public ActionResult Barcode(int productId, string barcode, short type)
        {
            IList<MenuOption> menuOptions = _menuOptionRepository.GetMany(o => o.IdMenu == productId).ToList();
            BarcodeModel model = new BarcodeModel()
                                     {
                                         OptionShows = menuOptions.Any()
                                                           ? (from menuOption in menuOptions
                                                              select new MenuOptionMapping.OptionShow()
                                                                         {
                                                                             id_ = menuOption.id_,
                                                                             Img = menuOption.Img,
                                                                             Barcode = menuOption.Barcode,
                                                                             Flag = menuOption.Flag,
                                                                             TenLoai = menuOption.TenLoai,
                                                                         }).ToList()
                                                           : new List<MenuOptionMapping.OptionShow>(),
                                         Barcode = barcode,
                                         Type = type
                                     };

            return View("ColorBarcode", model);
        }

        public JsonResult CheckBarcode(string code)
        {
            //kiem tra ma vach co ton tai trong he thong hay khong
            int somavach = _menuRepository.ChekBarcode(code);
            if (somavach == 0)
            {
                //ma vach khong ton tai
                return Json(new
                               {
                                   ok = false,
                                   message = "Mã vạch này không đúng"
                               }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var menu = _menuOptionRepository.Get(o => o.Barcode.Equals(code));
                if (menu != null)
                    return Json(new
                    {
                        ok = false,
                        message = "Mã vạch này đã tồn tại!"
                    }, JsonRequestBehavior.AllowGet);
            }
          
            return Json(new
                            {
                                ok = true
                            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckBarcodeMenuOption(string code)
        {
            int somavach = _menuRepository.ChekBarcode(code.Trim());
            if (somavach == 0)
            {
                //ma vach khong ton tai
                return Json(new
                {
                    ok = false,
                    message = "Mã vạch này không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var menu = _menuOptionRepository.Get(o => o.Barcode.Equals(code.Trim()));
                if (menu != null)
                    return Json(new
                    {
                        ok = false,
                        message = "Mã vạch này đã tồn tại!"
                    }, JsonRequestBehavior.AllowGet);
            }
          
            return Json(new
                            {
                                ok = true
                            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteMenuOption(int id)
        {
            _menuOptionRepository.Delete(o => o.id_ == id);
            _unitOfWork.Commit();
        }



        public ActionResult ThemMaVachMui(int productId)
        {
            IList<MenuOption> menuOptions = _menuOptionRepository.GetMany(o => o.IdMenu == productId).ToList();

            return View("mavachmui", menuOptions.Any()
                                         ? (from menuOption in menuOptions
                                            select new MenuOptionMapping.OptionShow()
                                                       {
                                                           id_ = menuOption.id_,
                                                           Img = menuOption.Img,
                                                           Barcode = menuOption.Barcode,
                                                           Flag = menuOption.Flag,
                                                           TenLoai = menuOption.TenLoai,
                                                       }).ToList()
                                         : new List<MenuOptionMapping.OptionShow>());
        }

        public ActionResult KhongCoGi(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<MenuOption> tags =
                _menuOptionRepository.GetMany(o => o.Flag == 0).OrderByDescending(o => o.SDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Khongcogi", data);
        }

        public ActionResult DeleteKhongCoGi(int id)
        {
            var idxoa = _menuOptionRepository.GetById(id);
            _menuOptionRepository.Delete(idxoa);

            //khi xoa ma vach thi update lai hasvalue
            //b1: get idmenu tu id cua menuoption
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand(
                    "update Menu set HasValue='false' where id_ = (select IdMenu from MenuOption where id_={0})", id);

            }
            _unitOfWork.Commit();
            return RedirectToAction("KhongCoGi");
        }

        public ActionResult EditMVKhongCoGi(int id)
        {
            var dlcu = _menuOptionRepository.GetById(id);
            return View("EditKhongCoGi", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditMVKhongCoGi(MenuOption model)
        {
            MenuOption menu = _menuOptionRepository.GetById(model.id_);
            menu.Barcode = model.Barcode;
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;

            _menuOptionRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("KhongCoGi");
        }

        public ActionResult Mau(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<MenuOption> tags =
                _menuOptionRepository.GetMany(o => o.Flag == 1).OrderByDescending(o => o.SDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("mau", data);
        }

        public ActionResult DeleteMau(int id)
        {
            var idxoa = _menuOptionRepository.GetById(id);
            _menuOptionRepository.Delete(idxoa);
            //khi xoa ma vach thi update lai hasvalue
            //b1: get idmenu tu id cua menuoption
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand(
                    "update Menu set HasValue='false' where id_ = (select IdMenu from MenuOption where id_={0})", id);

            }
            _unitOfWork.Commit();
            return RedirectToAction("Mau");
        }

        public ActionResult EditMVMau(int id)
        {
            var dlcu = _menuOptionRepository.GetById(id);
            return View("EditMaVachMau", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditMVMau(MenuOption model)
        {
            MenuOption menu = _menuOptionRepository.GetById(model.id_);
            menu.Barcode = model.Barcode;
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;
            menu.TenLoai = model.TenLoai;
            menu.Img = (model.Img).Replace("/files/", "");
            _menuOptionRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("Mau");
        }

        public ActionResult Mui(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<MenuOption> tags =
                _menuOptionRepository.GetMany(o => o.Flag == 2).OrderByDescending(o => o.SDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("mui", data);
        }

        public ActionResult DeleteMui(int id)
        {
            var idxoa = _menuOptionRepository.GetById(id);
            _menuOptionRepository.Delete(idxoa);
            //khi xoa ma vach thi update lai hasvalue
            //b1: get idmenu tu id cua menuoption
            using (var context = new ShopDataContex())
            {
                context.Database.ExecuteSqlCommand(
                    "update Menu set HasValue='false' where id_ = (select IdMenu from MenuOption where id_={0})", id);

            }
            _unitOfWork.Commit();
            return RedirectToAction("Mui");
        }

        public ActionResult EditMVMui(int id)
        {
            var dlcu = _menuOptionRepository.GetById(id);
            return View("EditMaVachMui", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditMVMui(MenuOption model)
        {
            MenuOption menu = _menuOptionRepository.GetById(model.id_);
            menu.Barcode = model.Barcode;
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;
            menu.TenLoai = model.TenLoai;
            menu.Img = (model.Img).Replace("/files/", "");
            _menuOptionRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("Mui");
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveBarcode(BarcodeSubmitModel model)
        {
            //them moi ma vach
            MenuOption menuOption = new MenuOption();
            if (model.Id == 0)
            {
                int somavach = _menuRepository.ChekBarcode(model.Barcode);
                if (somavach == 0)
                {
                    //ma vach khong ton tai
                    return Json(new
                    {
                        ok = false,
                        message = "Mã vạch này không đúng"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ma vach bi trung
                    var menu = _menuOptionRepository.Get(o => o.Barcode.Equals(model.Barcode));
                    if (menu != null)
                        return Json(new
                        {
                            ok = false,
                            message = "Mã vạch này đã tồn tại!"
                        }, JsonRequestBehavior.AllowGet);
                }
                if (model.Flag == 0)
                {
                    //khong co gi
                    //xoa du lieu trong bang voi Idmenu
                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("delete from MenuOption where MenuOption.IdMenu={0}", model.IdMenu);
                        context.Database.ExecuteSqlCommand("update Menu set Menu.HasValue='False'  where Menu.id_={0}", model.IdMenu);

                    }
                    menuOption = new MenuOption()
                     {
                         Barcode = model.Barcode,
                         Flag = model.Flag,
                         Img = "",
                         TenLoai = "Không có gì",
                         SDate = DateTime.Now,
                         sDateOk = DateTime.Now,
                         IdMenu = model.IdMenu

                     };
                    _menuOptionRepository.Add(menuOption);

                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("update Menu set Menu.HasValue='True'  where Menu.id_={0}", model.IdMenu);
                        context.Database.ExecuteSqlCommand("update Menu set Menu.BarcodeType={0} where Menu.id_={1}", model.Flag, model.IdMenu);

                    }

                }
                else
                {
                    // co mau hoac co mui
                    menuOption = new MenuOption()
                    {
                        Barcode = model.Barcode,
                        Flag = model.Flag,
                        Img = model.Hinh.Replace("/files/", ""),
                        TenLoai = model.TenLoai,
                        SDate = DateTime.Now,
                        sDateOk = DateTime.Now,
                        IdMenu = model.IdMenu
                    };
                    _menuOptionRepository.Add(menuOption);

                }
                //_menuOptionRepository.Add(menuOption);
                //sau khi them moi xong update lai barcodetype trong bang menu
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set Menu.HasValue='True'  where Menu.id_={0}", model.IdMenu);
                    context.Database.ExecuteSqlCommand("update Menu set Menu.BarcodeType={0} where Menu.id_={1}", model.Flag, model.IdMenu);

                }

            }

            _unitOfWork.Commit();
            //update xong kiem tra san pham do con hang hay het hang roi update lai
            int tonkho = _menuRepository.CheckOnhand(model.Barcode);
            int idsanpham = _menuOptionRepository.GetIdSanPhamByBarCode(model.Barcode);
            if (tonkho <= 0)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set HasOnHand='false' where id_ ={0}", idsanpham);

                }
            }

            return Json(new
            {
                ok = true,
                message = "Cập nhật mã vạch thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet, ValidateInput(false)]
        public JsonResult GetIdCon(int Idmenu)
        {
            //lay danh sach idcon vơi idmenu truyen tu ajax qua
            int[] DanhSachCon = _menuOptionRepository.GetIDMenuOption(Idmenu).ToArray();
            countTong = DanhSachCon.Length;
            // int[] danhSachIdCon = {};
            return Json(new
            {

                DanhSachCon
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CheckBarCode(string data, string ids)
        {
            var listBarCode = data.Split(',');
            var listIds = ids.Split(',');
            int Index = 0;
            var rsCheck = true;
            var mess = "Hợp lệ";
            foreach (var barcode in listBarCode)
            {
                Index++;
                var rs = _menuRepository.ChekBarcode(barcode.Trim());
                if (rs == 0)
                {
                    mess = "mã vạch không tồn tại";
                    rsCheck = false;
                    break;
                }
                else
                {
                    var menu = _menuOptionRepository.Get(o => o.Barcode.Equals(barcode.Trim()));
                    if (menu != null)
                    {
                        if (menu.id_ + "" == listIds[Index - 1])
                        {
                            continue;
                        }
                        mess = "mã vạch bị trùng";
                        rsCheck = false;
                        break;
                    }
                }
            }
            return Json(new
            {
                ok = rsCheck,
                //message = Index + "",
                message = " "+Index + " " + mess,//du lieu co van de dong thu bao nhieu
                //Dòng1mã vạch không tồn tại
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateData(List<BarcodeSubmitModel> listDataUpdate, List<BarcodeSubmitModel> listDataNew)
        {
            // them ma vach moi
            if (listDataNew != null)
            {
                foreach (var item in listDataNew)
                {
                    string image = "", tenloai = "";

                    if (item.Hinh != null)
                    {
                        image = item.Hinh.Replace("/files/", "");
                    }
                    if (item.TenLoai != null)
                    {
                        tenloai = item.TenLoai;
                    }
                    MenuOption menu = new MenuOption()
                    {
                        Barcode = string.IsNullOrEmpty(item.Barcode)?"":item.Barcode.Trim(),
                        Flag = item.Flag,
                        IdMenu = item.IdMenu,
                        Img = image,
                        TenLoai = tenloai,
                        SDate = DateTime.Now,
                        sDateOk = DateTime.Now

                    };
                    _menuOptionRepository.Add(menu);
                    _unitOfWork.Commit();
                    //sau khi them moi update trang thai cua menu va barodetype
                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("update Menu set Menu.HasValue='True'  where Menu.id_={0}", item.IdMenu);
                        context.Database.ExecuteSqlCommand("update Menu set Menu.BarcodeType={0} where Menu.id_={1}", item.Flag, item.IdMenu);

                    }
                }
            }

            if (listDataUpdate != null)
            {

                foreach (var item in listDataUpdate)
                {
                    string image = "", tenloai = "";

                    if (item.Hinh != null)
                    {
                        image = item.Hinh.Replace("/files/", "");
                    }
                    if (item.TenLoai != null)
                    {
                        tenloai = item.TenLoai;
                    }
                    MenuOption menuOption = new MenuOption();
                    //sua du lieu
                    menuOption = _menuOptionRepository.GetById(item.Id);
                    menuOption.TenLoai = tenloai;
                    menuOption.Barcode = string.IsNullOrEmpty(item.Barcode)?"":item.Barcode.Trim();
                    menuOption.Img = image;
                    menuOption.Flag = item.Flag;
                    menuOption.IdMenu = item.IdMenu;
                    _menuOptionRepository.Update(menuOption);
                    _unitOfWork.Commit();
                    using (var context = new ShopDataContex())
                    {
                        context.Database.ExecuteSqlCommand("update Menu set Menu.HasValue='True'  where Menu.id_={0}", item.IdMenu);
                        context.Database.ExecuteSqlCommand("update Menu set Menu.BarcodeType={0} where Menu.id_={1}", item.Flag, item.IdMenu);

                    }
                }
            }

            return Json(new
            {
                ok = true,
                message = "Cập nhật thành công!",
            }, JsonRequestBehavior.AllowGet);

        }
    }

    public class BarcodeSubmitModel
    {
        public int Id { get; set; }
        public string TenLoai { get; set; }
        public string Barcode { get; set; }
        public string Hinh { get; set; }
        public int Flag { get; set; }
        public int IdMenu { get; set; }
        public int count { get; set; }
    }
}
