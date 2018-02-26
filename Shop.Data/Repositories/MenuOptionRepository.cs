using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class MenuOptionRepository : RepositoryBase<MenuOption>, IMenuOptionRepository
    {
        public MenuOptionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        //truyen id cua ma vach de lay idmenu
        public int GetIdMenuFromBarcode(int idmavach)
        {
            string sql = string.Format("select IdMenu from MenuOption where id_={0}", idmavach);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //lay ma vach hien thi trang chi tiet
        public IList<MenuOption> GetBarcode(int idsanpham)
        {
            string sql = string.Format("select * from MenuOption where IdMenu={0}",idsanpham);
            return this.DataContext.Database.SqlQuery<MenuOption>(sql).ToList();
        }
        //lay anh mau mui hien thi trang chi tiet
        public IList<MenuOption> GetImgMauMui(int idsanpham)
        {
            string sql = string.Format("select * from MenuOption where IdMenu={0}",idsanpham);
            return this.DataContext.Database.SqlQuery<MenuOption>(sql).ToList();
        }
        //kiem tra san pham do co co ma vach mau hay mui de hien thi ra thanh tuy chon
        public int CheckMauOrMui(int idsanpham)
        {
            string sql = string.Format("select top 1 Flag from MenuOption where IdMenu={0}",idsanpham);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //lay ten san pham thong qua ma san pham trong bang menuoption
        public string GetTenSpByIdMenu(int id)
        {
            string sql = string.Format("select NameProduct from MenuOption where IdMenu={0}",id);
            return this.DataContext.Database.SqlQuery<string>(sql).First();
        }
        //lay id thong qua ma vach
        public int GetIdSanPhamByBarCode(string mavach)
        {
             string sql = string.Format("select IdMenu from MenuOption where Barcode='{0}'",mavach);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //kiem tra san pham do co ma vach hay chua de hien thi trang chi tiet
        public int CountMaVachByIdSanPham(int id)
        {
            string sql = string.Format("select COUNT(*) from MenuOption where MenuOption.IdMenu={0}",id);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //lay so luong dong cua ma vach trong bang menuoption
        public int LaySoLuongBarcodeInMenuOption(int idMenu)
        {
            string sql = string.Format("select COUNT(*) from MenuOption where IdMenu={0}",idMenu);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //lay id cua ma vach(tuc la id cua bang menuoption ung voi idmenu truyen vao)
        public int[] GetIDMenuOption(int idMenu)
        {
            string sql = string.Format("select id_ from MenuOption where MenuOption.IdMenu={0}", idMenu);
            return this.DataContext.Database.SqlQuery<int>(sql).ToArray();
        }
        //lay ma vach dau tien trong menuoption dua vao id san pham
        public string GetBarcodeFromIdSanPham(int idsanpham)
        {
            string sql = string.Format("select top 1 Barcode from MenuOption where MenuOption.IdMenu={0}",idsanpham);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        public string GetLinkSpByBarCode(string barcode)
        {
            string sql = string.Format("select Link from Menu where id_=(select top 1 IdMenu from MenuOption where Barcode='{0}')",barcode);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }

        public IList<PromotionDetailMapping.PromotionDetail>GetForPromotionDetailByCodes(IList<string>codes )
        {
            return (from menuOption in DataContext.MenuOptions.AsNoTracking()
                    join menu in DataContext.Menu.AsNoTracking() on menuOption.IdMenu equals menu.id_

                    where codes.Contains(menuOption.Barcode)
                    select new PromotionDetailMapping.PromotionDetail()
                               {
                                   Name = menu.NameProduct,
                                   PricePro = menu.PricePro,
                                   ProductId = menu.id_,
                                   Code = menuOption.Barcode
                               }).ToList();
        }
    }
    public interface IMenuOptionRepository : IRepository<MenuOption>
    {
        int GetIdMenuFromBarcode(int idmavach);
        IList<MenuOption> GetBarcode(int idsanpham);
        IList<MenuOption> GetImgMauMui(int idsanpham);
        int CheckMauOrMui(int idsanpham);
        string GetTenSpByIdMenu(int id);
        int GetIdSanPhamByBarCode(string mavach);
        int CountMaVachByIdSanPham(int id);
        int LaySoLuongBarcodeInMenuOption(int idMenu);
        int[] GetIDMenuOption(int idMenu);
        string GetBarcodeFromIdSanPham(int idsanpham);
        string GetLinkSpByBarCode(string barcode);

        IList<PromotionDetailMapping.PromotionDetail> GetForPromotionDetailByCodes(IList<string> codes);

    }
}
