using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class ProductStockSyncRepository : RepositoryBase<ProductStockSync>, IProductStockSyncRepository
    {
        public ProductStockSyncRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        //kiem tra ton kho theo chi nhanh dua vao id san pham
        public int KiemTraTonKhoChiNhanhDN(string Barcode)
        {
            string sql = string.Format("select OnHand from ProductStockSync where Idbranch=48610 and Barcode='{0}'", Barcode);//BG - Đà Nẵng
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //kiem tra ton kho theo chi nhanh dua vao id san pham
        public int KiemTraTonKhoChiNhanhHaNoi(string Barcode)
        {
            string sql = string.Format("select OnHand from ProductStockSync where Idbranch=48609 and Barcode='{0}'", Barcode);//BG - Hà Nội
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //kiem tra ton kho theo chi nhanh dua vao id san pham
        public int KiemTraTonKhoChiNhanhPhuNhuan(string Barcode)
        {
            string sql = string.Format("select OnHand from ProductStockSync where Idbranch=48611 and Barcode='{0}'", Barcode);//BG - Phú Nhuận
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //kiem tra ton kho theo chi nhanh dua vao id san pham
        public int KiemTraTonKhoChiNhanhQ3(string Barcode)
        {
            string sql = string.Format("select OnHand from ProductStockSync where Idbranch=14364 and Barcode='{0}'", Barcode);//Trung tâm Phân phối
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }

        public IList<ProductStockSyncMapping.TonKho> GetTonKhoCuaSanPham(int id)
        {
            return (from productStockSync in DataContext.ProductStockSync.AsNoTracking()
                where productStockSync.IdMenu == id
                select new ProductStockSyncMapping.TonKho
                {
                    Barcode = productStockSync.Barcode,
                    IdMenu = productStockSync.IdMenu,
                    Idbranch = productStockSync.Idbranch,
                    OnHand = productStockSync.OnHand
                }).ToList();
        } 
    }
    public interface IProductStockSyncRepository : IRepository<ProductStockSync>
    {
        int KiemTraTonKhoChiNhanhDN(string Barcode);
        int KiemTraTonKhoChiNhanhQ3(string Barcode);
        int KiemTraTonKhoChiNhanhPhuNhuan(string Barcode);
        int KiemTraTonKhoChiNhanhHaNoi(string Barcode);
        IList<ProductStockSyncMapping.TonKho> GetTonKhoCuaSanPham(int id);
    }
}
