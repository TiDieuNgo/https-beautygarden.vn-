using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
        {
        public CommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface ICommentRepository : IRepository<Comment>
    {
       
    }    
}
