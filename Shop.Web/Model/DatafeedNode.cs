using System.Collections.Generic;
using System.Linq;
using Shop.Data;

namespace Shop.Web.Model
{
    public class DatafeedNode
    {
        public bool Availability_instock { get; set; }
        public string Product_name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public int? Discounted_price { get; set; }
        public string Parent_of_cat_1 { get; set; }
        public string Category_1 { get; set; }
        public string Picture_url { get; set; }
        public string Url { get; set; }
        public string Promotion { get; set; }
        public string Delivery_period { get; set; }
        public string CategoryLv1 { get; set; }
        public string CategoryLv2 { get; set; }

        public static List<DatafeedNode> GetproductIdsListDatafeed()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<DatafeedNode>(@"select 
	                                                            ISNULL(cate_lv1.NameProduct,'') AS CategoryLv1
	                                                            ,ISNULL(cate_lv2.NameProduct,'') AS CategoryLv2
	                                                            ,product.NameProduct as Product_name
	                                                            ,ISNULL(product.Note, '') as Description
	                                                            ,product.PricePro as Price
	                                                            ,product.Link as Url
	                                                            ,product.Img as Picture_url
                                                            from Menu product  
                                                            left join Menu cate_lv1 ON cate_lv1.id_ = (select top 1 idControl from Menu where id_ in (select idMenuCatelogy from MenuProAdd where idMenuProAdded = product.id_))
                                                            left join Menu cate_lv2 ON cate_lv2.id_ = (select TOP 1 idMenuCatelogy from MenuProAdd where idMenuProAdded = product.id_) 
                                                            where product.idcontrol=11 and product.HasValue=1 and product.ok=1 and product.HasOnHand=1").ToList();
                return data;
            }
        }
    }
}