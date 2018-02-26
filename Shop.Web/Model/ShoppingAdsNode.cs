using System.Collections.Generic;
using System.Linq;
using Shop.Data;

namespace Shop.Web.Model
{
    public class ShoppingAdsNode
    {
        public string Productname { get; set; }
        public string UrlSp { get; set; }
        public string CompanyName { get; set; }
        public string UrlHome { get; set; }
        public string images { get; set; }
        public int? price { get; set; }
        public int? saleprice { get; set; }
        public string keyword { get; set; }
        public int region { get; set; }

        //public static List<DatafeedNode> GetproductIdsListDatafeed()
        //{
        //    using (var context = new ShopDataContex())
        //    {
        //        var data = context.Database.SqlQuery<DatafeedNode>(@"select 
	       //                                                     ISNULL(cate_lv1.NameProduct,'') AS CategoryLv1
	       //                                                     ,ISNULL(cate_lv2.NameProduct,'') AS CategoryLv2
	       //                                                     ,product.NameProduct as Product_name
	       //                                                     ,ISNULL(product.Note, '') as Description
	       //                                                     ,product.PricePro as Price
	       //                                                     ,product.Link as Url
	       //                                                     ,product.Img as Picture_url
        //                                                    from Menu product  
        //                                                    left join Menu cate_lv1 ON cate_lv1.id_ = (select top 1 idControl from Menu where id_ in (select idMenuCatelogy from MenuProAdd where idMenuProAdded = product.id_))
        //                                                    left join Menu cate_lv2 ON cate_lv2.id_ = (select TOP 1 idMenuCatelogy from MenuProAdd where idMenuProAdded = product.id_) 
        //                                                    where product.idcontrol=11 and product.HasValue=1 and product.ok=1 and product.HasOnHand=1").ToList();
        //        return data;
        //    }
        //}
    }
}