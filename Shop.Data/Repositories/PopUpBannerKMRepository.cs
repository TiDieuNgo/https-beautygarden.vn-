using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class PopUpBannerKMRepository : RepositoryBase<PopUpBannerKM>, IPopUpBannerKMRepository
    {
        public PopUpBannerKMRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IPopUpBannerKMRepository : IRepository<PopUpBannerKM>
    {
       
    }    
}
