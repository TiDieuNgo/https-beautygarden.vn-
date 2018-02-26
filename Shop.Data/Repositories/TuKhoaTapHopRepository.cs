using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class TuKhoaTapHopRepository : RepositoryBase<TuKhoaTapHop>, ITuKhoaTapHopRepository
        {
        public TuKhoaTapHopRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ITuKhoaTapHopRepository : IRepository<TuKhoaTapHop>
    {
       
    }    
}
