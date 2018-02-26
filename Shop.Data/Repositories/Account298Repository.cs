using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class Account298Repository : RepositoryBase<Account298>, IAccount298Repository
        {
        public Account298Repository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IAccount298Repository : IRepository<Account298>
    {
       
    }    
}
