using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class BannerDanhSachRepository : RepositoryBase<BannerDanhSach>, IBannerDanhSachRepository
        {
        public BannerDanhSachRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IBannerDanhSachRepository : IRepository<BannerDanhSach>
    {
       
    }    
}
