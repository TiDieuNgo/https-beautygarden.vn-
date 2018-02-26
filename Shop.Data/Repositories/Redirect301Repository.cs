using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class Redirect301Repository : RepositoryBase<Redirect301>, IRedirect301Repository
    {
        public Redirect301Repository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IRedirect301Repository : IRepository<Redirect301>
    {
       
    }    
}
