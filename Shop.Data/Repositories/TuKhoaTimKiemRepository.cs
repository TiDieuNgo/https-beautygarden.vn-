using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class TuKhoaTimKiemRepository : RepositoryBase<TuKhoaTimKiem>, ITuKhoaTimKiemRepository
        {
        public TuKhoaTimKiemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
        public IList<TuKhoaTimKiem> DanhSachTuKhoa()
       {
           string sql = string.Format("select top 10 * from TuKhoaTimKiems order by CountRow desc");
           return this.DataContext.Database.SqlQuery<TuKhoaTimKiem>(sql).ToList();
       }

      
        
        }
    public interface ITuKhoaTimKiemRepository : IRepository<TuKhoaTimKiem>
    {
        IList<TuKhoaTimKiem> DanhSachTuKhoa();
    }    
}
