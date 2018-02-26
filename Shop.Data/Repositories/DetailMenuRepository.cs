using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class DetailMenuRepository : RepositoryBase<DetailMenu>, IDetailMenuRepository
    {
        public DetailMenuRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IList<DetailMenu> TimKiem(string key)
        {
            string sql =
                string.Format("select * from DetailMenu where LOWER(CodeName) LIKE '%{0}' Or LOWER(CodeName) LIKE '{0}%' or LOWER(CodeName) like '%{0}%' OR LOWER(Name) LIKE '%{0}' Or LOWER(Name) LIKE '{0}%' or LOWER(Name) like '%{0}%'", key);
            return this.DataContext.Database.SqlQuery<DetailMenu>(sql).ToList();
        }
        public IList<TinTucMapping> GetTenDanhMuc(int id)
        {
            return (from tintuc in DataContext.DetailMenu
                    join danhmuctintuc in DataContext.Menu on tintuc.id_ equals danhmuctintuc.id_
                    where tintuc.id_ == id
                    select new TinTucMapping()
                    {
                        DanhMucTinTucId = tintuc.id_,
                        sDate = DateTime.Now,
                        id_ = tintuc.id_,
                        TenDanhMuc = danhmuctintuc.NameProduct,
                        Name = tintuc.Name,
                        Note = tintuc.Note
                    }).ToList();
        }
        public int GetIdTinTuc(string link)
        {
            string sql = string.Format("select id_ from DetailMenu where DetailMenu.Link='{0}'", link);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        public IList<TagName> GetTagName()
        {
            string sql = string.Format("select TenTag from TagTinTucs where TenTag is not null");
            return this.DataContext.Database.SqlQuery<TagName>(sql).ToList();
        }
        public IList<TagName> GetTagEdit(int id)
        {
            string sql = string.Format("select TenTag from TagTinTucs where IdMenu = {0}", id);
            return this.DataContext.Database.SqlQuery<TagName>(sql).ToList();
        }
        public IList<DetailMenu> LayTatCaTinTucTheoTag(string taglink)
        {
            string sql =
                string.Format(@"select 
	                             M.*
	                             from DetailMenu M 
	                             inner join TagTinTucs tag on M.id_=tag.IdMenu
	                              where tag.Link = '{0}'", taglink);
            return this.DataContext.Database.SqlQuery<DetailMenu>(sql).ToList();
        }

        public string GettagNameBytaglink(string taglink)
        {
            string sql = string.Format("select top 1 TenTag from TagTinTucs where Link = '{0}'",taglink);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
    }
    public interface IDetailMenuRepository : IRepository<DetailMenu>
    {
        IList<DetailMenu> TimKiem(string key);
        IList<TinTucMapping> GetTenDanhMuc(int id);
        int GetIdTinTuc(string link);
        IList<TagName> GetTagName();
        IList<TagName> GetTagEdit(int id);
        IList<DetailMenu> LayTatCaTinTucTheoTag(string taglink);
        string GettagNameBytaglink(string taglink);
    }
}
