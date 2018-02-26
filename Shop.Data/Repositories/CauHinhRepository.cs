using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class CauHinhRepository : RepositoryBase<CauHinh>, ICauHinhRepository
        {
        public CauHinhRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ICauHinhRepository : IRepository<CauHinh>
    {
       
    }    
}
