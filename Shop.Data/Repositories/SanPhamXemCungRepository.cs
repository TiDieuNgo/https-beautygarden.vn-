using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class SanPhamXemCungRepository : RepositoryBase<SanPhamXemCung>, ISanPhamXemCungRepository
    {
        public SanPhamXemCungRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }

        public SanPhamXemCung Getdatatrung(int idsanpham, int idsanphamtiep)
        {
            string sql = string.Format("select * from SanPhamXemCungs where IdSanPham={0} and IdSanPhamTiepTheo={1}", idsanpham,idsanphamtiep);
            return this.DataContext.Database.SqlQuery<SanPhamXemCung>(sql).FirstOrDefault();
        }

        public int GetslTrung(int idsanpham, int idsanphamtiep)
        {
            string sql = string.Format("select count(*) from SanPhamXemCungs where IdSanPham={0} and IdSanPhamTiepTheo={1}", idsanpham, idsanphamtiep);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }

        public IList<int> danhsachIds(int idspdangxem)
        {
            string sql = string.Format("select top 8  IdSanPhamTiepTheo  from SanPhamXemCungs  where IdSanPham = {0} order by SoLanXem desc",idspdangxem);
            return this.DataContext.Database.SqlQuery<int>(sql).ToList();
        }
   }
    public interface ISanPhamXemCungRepository : IRepository<SanPhamXemCung>
    {
        SanPhamXemCung Getdatatrung(int idsanpham, int idsanphamtiep);
        int GetslTrung(int idsanpham, int idsanphamtiep);
        IList<int> danhsachIds(int idspdangxem);
    }    
}
