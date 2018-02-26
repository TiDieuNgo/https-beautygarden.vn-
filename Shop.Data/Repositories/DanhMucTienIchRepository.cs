using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class DanhMucTienIchRepository : RepositoryBase<DanhMucTienIch>, IDanhMucTienIchRepository
        {
        public DanhMucTienIchRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
            public string GetTenDanhMucById(int id)
            {
                string sql = string.Format("select TenDanhMuc from DanhMucTienIchs where id_={0}",id);
                return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
          public int GetIdDanhMucCha(int id)
            {
                string sql = string.Format("select Idcontrol from DanhMucTienIchs where id_={0}",id);
                return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
        }
    public interface IDanhMucTienIchRepository : IRepository<DanhMucTienIch>
    {
        string GetTenDanhMucById(int id);
        int GetIdDanhMucCha(int id);
    }    
}
