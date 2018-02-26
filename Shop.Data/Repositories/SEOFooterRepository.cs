using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class SEOFooterRepository : RepositoryBase<SEOFooter>, ISEOFooterRepository
    {
        public SEOFooterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ISEOFooterRepository : IRepository<SEOFooter>
    {
       
    }    
}
