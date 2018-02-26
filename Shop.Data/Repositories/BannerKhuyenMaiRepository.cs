using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class BannerKhuyenMaiRepository : RepositoryBase<BannerKhuyenMai>, IBannerKhuyenMaiRepository
    {
        public BannerKhuyenMaiRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IBannerKhuyenMaiRepository : IRepository<BannerKhuyenMai>
    {
       
    }    
}
