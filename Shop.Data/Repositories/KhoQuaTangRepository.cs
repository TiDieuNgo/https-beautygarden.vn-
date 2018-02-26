using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class KhoQuaTangRepository : RepositoryBase<KhoQuaTang>, IKhoQuaTangRepository
    {
        public KhoQuaTangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IKhoQuaTangRepository : IRepository<KhoQuaTang>
    {
       
    }    
}
