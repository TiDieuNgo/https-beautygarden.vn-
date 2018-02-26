using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class KhachHangLienHeRepository : RepositoryBase<AccMember298>, IAccMember298Repository
        {
        public KhachHangLienHeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IAccMember298Repository : IRepository<AccMember298>
    {
       
    }    
}
