using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class YKienKhachHangRepository : RepositoryBase<YKienKhachHang>, IYKienKhachHangRepository
        {
        public YKienKhachHangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IYKienKhachHangRepository : IRepository<YKienKhachHang>
    {
       
    }    
}
