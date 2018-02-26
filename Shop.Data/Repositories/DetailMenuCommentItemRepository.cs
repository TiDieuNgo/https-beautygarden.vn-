using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class DetailMenuCommentItemRepository : RepositoryBase<DetailMenuCommentItem>, IDetailMenuCommentItemRepository
        {
        public DetailMenuCommentItemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IDetailMenuCommentItemRepository : IRepository<DetailMenuCommentItem>
    {
       
    }    
}
