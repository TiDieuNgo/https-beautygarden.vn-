<%@ WebHandler Language="C#" Class="HandlerAutocomplete" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Linq;
using Shop.Data;
using Shop.Web.Model;

public class HandlerAutocomplete : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string keyword = context.Request.QueryString["keyword"];
        context.Response.ContentType = "application/json";
        using (var db = new ShopDataContex())
        {
            var rs =
                db.Menu.Where(v => v.idControl == 11 & v.ok == true & v.HasValue == true &v.HasOnHand & (v.CodeProduct.Contains(keyword) || v.NameProduct.Contains(keyword))).Select(v => new ResultSearch()
                                                                                                          {
                                                                                                              image =
                                                                                                                  v.Img,
                                                                                                              label =
                                                                                                                  v.
                                                                                                                  NameProduct,
                                                                                                              value =
                                                                                                                  v.id_,
                                                                                                              price =
                                                                                                                  v.
                                                                                                                  PricePro,
                                                                                                              link = v.Link
                                                                                                          }).Take(10).ToList();
            context.Response.Write(new JavaScriptSerializer().Serialize(rs));
        }

        System.Threading.Thread.Sleep(200);
        YKienKhachHangModel.TukhoaTimkiemAdd(keyword);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

class ResultSearch
{
    public string label { get; set; }
    public decimal value { get; set; }
    public string image { get; set; }
    public double? price { get; set; }
    public string link { get; set; }

}