using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Model
{
    public class CommentModel
    {
        public IList<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
        public IList<TraLoiComment> TraLoiComments { get; set; }

        public static int GetSlCautraloi(int idcmt)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select Count(*) from TraLoiComments where CommentId={0}", idcmt);
                return context.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
        }
    }
}