using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class ThongBaoKhiCoHangRepository : RepositoryBase<ThongBaoKhiCoHang>, IThongBaoKhiCoHangRepository
    {
        public ThongBaoKhiCoHangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
        public IList<ThongBaoKhiCoHang> GetSanPhamConHangs()
        {
            string sql = string.Format("select * from ThongBaoKhiCoHangs where ProductId in (select id_ from Menu where HasOnHand = 1)");
            return this.DataContext.Database.SqlQuery<ThongBaoKhiCoHang>(sql).ToList();
        }
    }
    public interface IThongBaoKhiCoHangRepository : IRepository<ThongBaoKhiCoHang>
    {
        IList<ThongBaoKhiCoHang> GetSanPhamConHangs();
    }    
}
