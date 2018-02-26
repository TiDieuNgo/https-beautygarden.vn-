using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class EmailRepository : RepositoryBase<Email>, IEmailRepository
        {
        public EmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IEmailRepository : IRepository<Email>
    {
       
    }    
}
