using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class TraLoiCommentRepository : RepositoryBase<TraLoiComment>, ITraLoiCommentRepository
        {
        public TraLoiCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ITraLoiCommentRepository : IRepository<TraLoiComment>
    {
       
    }    
}
