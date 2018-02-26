using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class IframeSalepageRepository : RepositoryBase<IframeSalepage>, IIframeSalepageRepository
    {
        public IframeSalepageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IIframeSalepageRepository : IRepository<IframeSalepage>
    {
       
    }    
}
