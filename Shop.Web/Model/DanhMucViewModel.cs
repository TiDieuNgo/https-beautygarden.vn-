using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Models
{
    public class DanhMucViewModel
    {
        public IList<Menu> DanhMucs { get; set; }
        public Menu DanhMuc { get; set; }
        public int id { get; set; }
        public int ParnetId { get; set; }
        public string BannerDanhMuc { get; set; }
        public bool HasPromotion { get; set; }
    }
    public class DanhMucTree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool Selected { get; set; }
        public IList<DanhMucTree> Childs { get; set; }
    }
    public class DanhMucGetData
    {
        public static int GetslHientai(int idanhmuc)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format(@"DECLARE @id INT = " + idanhmuc + @"
                SELECT 
	                COUNT(DISTINCT A.idMenuProAdded) AS CountProduct
                FROM  Menu B
                LEFT JOIN MenuProAdd A ON A.idMenuProAdded = B.id_
                WHERE (
		                A.idMenuCatelogy = @id
		                OR idMenuCatelogy IN ( SELECT id_ FROM Menu WHERE idControl = @id )
		                OR idMenuCatelogy IN ( SELECT id_ FROM Menu WHERE idControl IN ( SELECT id_ FROM Menu WHERE idControl = @id ))
	                )
	                AND B.NameProduct IS NOT NULL 
	                AND B.ok = 1 AND HasValue = 1 AND HasOnHand = 1");
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
    }
}