using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class DanhSachTagRepository : RepositoryBase<DanhSachTag>, IDanhSachTagRepository
    {
        public DanhSachTagRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IList<TagName> GetTagName()
        {
            string sql = string.Format("select tenTag from DanhSachTags where TenTag is not null");
            return this.DataContext.Database.SqlQuery<TagName>(sql).ToList();
        }
        public IList<TagName> GetTagEdit(int idsanpham)
        {
            string sql = string.Format("select TenTag from DanhSachTags where idmenu={0}", idsanpham);
            return this.DataContext.Database.SqlQuery<TagName>(sql).ToList();
        }
        public IList<DanhSachTag> GetTenTagViewDetail(string splink)
        {
            string sql = string.Format("select * from DanhSachTags where IdMenu=(select id_ from Menu where link='{0}')",splink);
            return this.DataContext.Database.SqlQuery<DanhSachTag>(sql).ToList();
        }
        public int GetTagNamebyIdmenu(int idMenu, string tagname)
        {
            string sql = string.Format("select COUNT(*) from DanhSachTags where IdMenu={0} and TenTag=N'{1}'",idMenu,tagname);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }

        public IList<DanhSachTag> GetAllDanhSachTags()
        {
            string sql = string.Format(@"select * from danhsachtags A
                                        left join Menu B on A.IdMenu = B.id_ 
                                        where NameProduct is not null
                                        and A.DanhMucTag = 1");
            return this.DataContext.Database.SqlQuery<DanhSachTag>(sql).ToList();
        }
        public IList<DanhSachTag> GetDanhSachTagTinTucs()
        {
            string sql = string.Format(@"select * from danhsachtags A
                                        left join DetailMenu B on A.IdMenu = B.id_ 
                                        where B.Name is not null
                                        and A.DanhMucTag = 2");
            return this.DataContext.Database.SqlQuery<DanhSachTag>(sql).ToList();
        }

        public DanhSachTag GetSeoTag(string tentag)
        {
            string sql = string.Format("select top 1 * from danhsachtags where TenTag = N'{0}'", tentag);
            return this.DataContext.Database.SqlQuery<DanhSachTag>(sql).FirstOrDefault();
        }
        public IList<DanhSachTag> TimKiem(string key)
        {
            string sql =
                string.Format("select * from DanhSachTags where LOWER(Code) LIKE '%{0}' Or LOWER(Code) LIKE '{0}%' or LOWER(Code) like '%{0}%' OR LOWER(TenTag) LIKE '%{0}' Or LOWER(TenTag) LIKE '{0}%' or LOWER(TenTag) like '%{0}%'", key);
            return this.DataContext.Database.SqlQuery<DanhSachTag>(sql).ToList();
        }
    }
    public interface IDanhSachTagRepository : IRepository<DanhSachTag>
    {
        IList<TagName> GetTagName();
        IList<TagName> GetTagEdit(int idsanpham);
        IList<DanhSachTag> GetTenTagViewDetail(string splink);
        int GetTagNamebyIdmenu(int idMenu,string tagname);
        IList<DanhSachTag> GetAllDanhSachTags();
        IList<DanhSachTag> GetDanhSachTagTinTucs();
        DanhSachTag GetSeoTag(string tentag);
        IList<DanhSachTag> TimKiem(string key);
    }
}
