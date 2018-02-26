using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class MenuProAddRepository : RepositoryBase<MenuProAdd>, IMenuProAddRepository
    {
        public MenuProAddRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IList<SanPhamTheoDanhMucMapping> GetDaTaSanPham(int danhmuc)
        {
            //SELECT * FROM Menu INNER JOIN MenuOption ON Menu.id_=MenuOption.IdMenu
            return (from menu in DataContext.Menu
                    join menuproadd in DataContext.MenuProAdd on menu.id_ equals menuproadd.idMenuCatelogy
                    where menuproadd.idMenuCatelogy == danhmuc
                    select new SanPhamTheoDanhMucMapping()
                               {
                                   IconMenu = menu.IconMenu,
                                   HasOnHand = menu.HasOnHand,
                                   HasValue = menu.HasValue,
                                   Img = menu.Img,
                                   LevelMenu = menu.LevelMenu,
                                   NameProduct = menu.NameProduct,
                                   PriceOffPro = menu.PriceOffPro,
                                   PricePro = menu.PricePro,
                                   idControl = menu.idControl,
                                   id_ = menu.id_,
                                   ok = menu.ok,
                                   sDate = DateTime.Now,
                                   idMenuCatelogy = menuproadd.idMenuCatelogy,
                                   idMenuProAdded = menuproadd.idMenuProAdded
                               }).ToList();


        }
    }
    public interface IMenuProAddRepository : IRepository<MenuProAdd>
    {
        //lay du lieu san pham thong qua idmen
        IList<SanPhamTheoDanhMucMapping> GetDaTaSanPham(int danhmuc);
    }
}
