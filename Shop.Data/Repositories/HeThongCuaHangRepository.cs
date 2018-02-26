using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class HeThongCuaHangRepository : RepositoryBase<HeThongCuaHang>, IHeThongCuaHangRepository
    {
        public HeThongCuaHangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IHeThongCuaHangRepository : IRepository<HeThongCuaHang>
    {
       
    }    
}
