using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
        {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IUserRepository : IRepository<User>
    {
       
    }    
}
