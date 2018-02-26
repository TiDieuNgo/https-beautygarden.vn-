using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class SalePageRepository : RepositoryBase<SalePage>, ISalePageRepository
    {
        public SalePageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ISalePageRepository : IRepository<SalePage>
    {
       
    }    
}
