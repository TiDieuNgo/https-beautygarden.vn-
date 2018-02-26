using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class DetailMenuCommentRepository : RepositoryBase<DetailMenuComment>, IDetailMenuCommentRepository
    {
        public DetailMenuCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public int InsertIntoKHLH(string sql,string phone)
        {
            int id = 0;
           int row= this.DataContext.Database.ExecuteSqlCommand(sql);
            if(row!=0)
            {
                string sql1 = "select top 1 id from [bg.hvnet.vn].dbo.KH_LH WHERE Phone = '" + phone + "' order by id desc";
                id = DataContext.Database.SqlQuery<int>(sql1).FirstOrDefault();

            }
            return id;
        }

        public void InsertKHLHProduct(IList<KHLHProduct> list)
        {
            foreach (var o in list)
            {
                string sql =
                    string.Format(
                        "insert into [bg.hvnet.vn].dbo.KH_LH_Product (idKH,Code,SL,GiaWeb,CreatedAt,LinkImage,Quatang) values ({0},'{1}',{2},{3},'{4}','{5}','{6}')",
                        o.IdKH, o.Code, o.SL, o.GiaWeb, o.NgayTao.ToString("yyyy-MM-dd HH:mm:ss"),o.LinkImage,o.Quatang);
                this.DataContext.Database.ExecuteSqlCommand(sql);
            }
        }
    }
    public interface IDetailMenuCommentRepository : IRepository<DetailMenuComment>
    {
        int InsertIntoKHLH(string sql,string phone);
        void InsertKHLHProduct(IList<KHLHProduct> list);
    }
}
