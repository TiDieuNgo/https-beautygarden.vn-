using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
        {
        public ReviewRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
       
        }
    public interface IReviewRepository : IRepository<Review>
    {
       
    }    
}
