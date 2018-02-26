using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class NoiDungTienIchRepository : RepositoryBase<NoiDungTienIch>, INoiDungTienIchRepository
        {
        public NoiDungTienIchRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface INoiDungTienIchRepository : IRepository<NoiDungTienIch>
    {
       
    }    
}
