using System;
using System.Web;
using System.Collections.Generic;

namespace Shop.Web.Core.Helper
{
   public class CookieHelper
   {

    public static string CookieName {get;set;}
    public CookieHelper(string name)
    {
        CookieName = "BTS_" + name;
    }

    public void SetWishListCookie(string list)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["WishList"] = list;
        myCookie.Expires = DateTime.Now.AddDays(2);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
    public void SetCartCookie(string json)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["Cart"] = json;
        myCookie.Expires = DateTime.Now.AddDays(2);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
    //tao cookie cho san pham da xem
    public void SetUserInforCookie(string json)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["User_Infor"] = json;
        myCookie.Expires = DateTime.Now.AddYears(1);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
    //tao cookie cho user
    public void SetViewedCookie(string json)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["product_watched"] = json;
        myCookie.Expires = DateTime.Now.AddDays(2);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
    //tao cookie cho user khi dat hang
    public void SetUserOrderCookie(string json)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["User_Order"] = json;
        myCookie.Expires = DateTime.Now.AddYears(1);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
       //tao cookie sản phẩm xem cùng theo thuật toán
       public void SetViewedCookieXemCung(string json)
       {
           HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
           myCookie.Values["product_xemcung"] = json;
           myCookie.Expires = DateTime.Now.AddDays(2);
           HttpContext.Current.Response.Cookies.Add(myCookie);
       }
        public void SetViewCookie(string json)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
        myCookie.Values["View"] = json;
        myCookie.Expires = DateTime.Now.AddDays(2);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
   
   
    public HttpCookie GetCookie()
    {
        if (HttpContext.Current != null)
            return HttpContext.Current.Request.Cookies[CookieName] ?? null;
        return null;
    }

    public void ClearCookie()
    {
        HttpCookie myCookie= HttpContext.Current.Request.Cookies[CookieName];
        myCookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }

    public void ClearCookie(string _CookieName)
    {
        HttpCookie myCookie = HttpContext.Current.Request.Cookies[_CookieName];
        myCookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(myCookie);
    }
   }

}
