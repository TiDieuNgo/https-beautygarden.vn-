using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class TagTinTucRepository : RepositoryBase<TagTinTuc>, ITagTinTucRepository
    {
        public TagTinTucRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public int GetTagNamebyIdmenu(int idMenu, string tagname)
        {
            string sql = string.Format("select COUNT(*) from TagTinTucs where IdMenu={0} and TenTag=N'{1}'", idMenu, tagname);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        public IList<TagTinTuc> GetTenTagForTinTuc(string seolink)
        {
            string sql = string.Format("select * from TagTinTucs where IdMenu=(select top 1 id_ from DetailMenu where link='{0}')", seolink);
            return this.DataContext.Database.SqlQuery<TagTinTuc>(sql).ToList();
        }
        public IList<TagTinTuc> GetTenTagForReview(string seolink)
        {
            string sql = string.Format("select * from TagTinTucs where IdMenu=(select top 1 id_ from Reviews where Link='{0}')",seolink);
            return this.DataContext.Database.SqlQuery<TagTinTuc>(sql).ToList();
        }
        public IList<TagTinTuc> TimKiem(string key)
        {
            string sql =
                string.Format("select * from TagTinTucs where LOWER(Code) LIKE '%{0}' Or LOWER(Code) LIKE '{0}%' or LOWER(Code) like '%{0}%' OR LOWER(TenTag) LIKE '%{0}' Or LOWER(TenTag) LIKE '{0}%' or LOWER(TenTag) like '%{0}%'", key);
            return this.DataContext.Database.SqlQuery<TagTinTuc>(sql).ToList();
        }
    }
    public interface ITagTinTucRepository : IRepository<TagTinTuc>
    {
        int GetTagNamebyIdmenu(int idMenu, string tagname);
        IList<TagTinTuc> GetTenTagForTinTuc(string seolink);
        IList<TagTinTuc> GetTenTagForReview(string seolink);
        IList<TagTinTuc> TimKiem(string key);
    }
}
