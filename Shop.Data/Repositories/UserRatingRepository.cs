using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class UserRatingRepository : RepositoryBase<UserRating>, IUserRatingRepository
        {
        public UserRatingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }
        public  int GetLuotReView(string splink)
        {
            string sql = string.Format("select count(Id) from UserRatings where IdSanPham = (select id_ from Menu where Link='{0}')",splink);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        public double GetSao(string splink)
        {
            string sql = string.Format("SELECT  ISNULL(Round(SUM(Rating)/COUNT(Id), 1),1) FROM UserRatings where IdSanPham = (select id_ from Menu where Link='{0}')", splink);
            return this.DataContext.Database.SqlQuery<double>(sql).First();
        }
       
        }
    public interface IUserRatingRepository : IRepository<UserRating>
    {
        int GetLuotReView(string splink);
        double GetSao(string splink);
    }    
}
