using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class ThietLapRepository : RepositoryBase<ThietLap>, IThietLapRepository
        {
        public ThietLapRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IThietLapRepository : IRepository<ThietLap>
    {
       
    }    
}
