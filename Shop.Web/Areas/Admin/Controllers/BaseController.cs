using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Models;

namespace Shop.Web.Controllers
{
     
    public class BaseController : Controller
    {
        public string GetUserEmail(HttpContextBase context)
        {
            string code = GetCookieLogin(context);
            return code;
        }
        public string GetCookieLogin(HttpContextBase context)
        {
            HttpCookie cookie = context.Request.Cookies["UserLogin"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return string.Empty;
            return cookie.Value;
        }
        public void SetCookieLogin(RequestContext requestContext, string user)
        {
            requestContext.HttpContext.Response.Cookies.Add(
                new HttpCookie("UserLogin", user)
                {
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true,
                }
            );


        }
        public string RejectMarks(string text)
        {
            text = text.ToLower();
            string[] pattern = new string[7];

            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";

            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";

            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";

            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";

            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";

            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";

            pattern[6] = "d|đ";

            for (int i = 0; i < pattern.Length; i++)
            {
                // kí tự sẽ thay thế

                char replaceChar = pattern[i][0];

                MatchCollection matchs = Regex.Matches(text, pattern[i]);

                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            text = Regex.Replace(text, @"[^\w\.@]", " ").Trim().ToLower();
            text = text.Replace(" ", " ");
            return text;
        }
        //dùng để lưu link "son-moi-cao-cap" xuống DB
        public static string ConvertFont(string str_Convert)
        {
            str_Convert = str_Convert.ToLower().TrimStart().TrimEnd().Replace('đ', 'd');
            str_Convert = Regex.Replace(str_Convert, @"([áàảãạăắằẳẵặâấầẩẫậ]+)", "a", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([óòỏõọôốồổỗộơớờởỡợ]+)", "o", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([éèẻẽẹêếềểễệ]+)", "e", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([úùủũụưứừửữự]+)", "u", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([íìỉĩị]+)", "i", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([ýỳỷỹỵ]+)", "y", RegexOptions.IgnoreCase);
            str_Convert = Regex.Replace(str_Convert, @"([^0-9a-zA-Z- ]+)", "", RegexOptions.IgnoreCase);
            str_Convert = str_Convert.Replace(' ', '-').TrimEnd('-').Replace("---", "-").Replace("--", "-");

            return str_Convert;
        }
        public string CutChuoi(object chuoi)
        {
            string temp = "";
            string[] arr = chuoi.ToString().Split();

            if (arr.Length <= 5) return chuoi + string.Empty;

            for (int i = 0; i < 5; i++) temp += arr[i] + " ";


            return temp.TrimEnd() + "...";
        }
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
        public bool CheckRole(HttpContextBase context, int role)
        {
            ShopUser userLogin = context.User.GetShopUser();
            IList<int> ids = !string.IsNullOrEmpty(userLogin.RoleName)
                                        ? userLogin.RoleName.Split(',').Select(o => Convert.ToInt32(o)).ToList()
                                        : new List<int>();
            return ids.Contains(role);
        }
        public string ChangeImageSEO(object strContentInput, string TitleOrName, string Link)
        {
            try
            {
                int index = 0;
                string strContent = HttpUtility.HtmlDecode(strContentInput + string.Empty);

                // Kiểm tra tồn tại hình ảnh cần xử lý
                if (strContent.ToLower().IndexOf(".jpg") > 0 || strContent.ToLower().IndexOf(".png") > 0 || strContent.ToLower().IndexOf(".gif") > 0 || strContent.ToLower().IndexOf(".jpeg") > 0)
                {
                    // Xóa tất cả các thẻ A-Href bao ngoài thẻ IMG trước khi xử lý
                    strContent = Regex.Replace(strContent, "<a (.*?)><img (.*?)/></a>", "<img $2/>", RegexOptions.IgnoreCase);

                    // Duyệt tất cả các thẻ IMG cần thêm thẻ A-Href
                    foreach (Match match in Regex.Matches(strContent, "<img (.*?) />", RegexOptions.IgnoreCase))
                    {
                        // Lấy đường dẫn của file IMG
                        Match mSrc = Regex.Match(match.Value, "src=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match mAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match mTitle = Regex.Match(match.Value, "title=\"(.*?)\"");

                        string imgSrc = mSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn", "").Replace("http://beautygarden.vn", "").Replace("\"", "");

                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcPath = Server.MapPath(imgSrc);

                            // Kiểm tra tồn tại file hình ảnh trên hoting này ko?
                            if (System.IO.File.Exists(imgSrcPath))
                            {
                                index++;
                                System.Drawing.Image img = System.Drawing.Image.FromFile(imgSrcPath);

                                // Chỉ tạo link về chính hình ảnh hiện tại cho những hình ảnh lớn và thỏa điều kiện
                                //if (img.Width >= 100 && img.Height >= 50)
                                //{
                                    img.Dispose();
                                    string imgName = "", imgPath = "", extName = ".jpg", imgNameNew = "", imgNameChanged = "", imgConvert = "";
                                    string[] arr = imgSrc.Split('/');
                                    imgName = arr[arr.Length - 1];
                                    imgPath = imgSrc.Replace(imgName, "");
                                    arr = imgName.Split('.');
                                    extName = arr[arr.Length - 1];
                                    imgNameNew = string.Format("{0}{1}-hinh-anh-{2}.{3}", imgPath, Link, index, extName);
                                    imgNameChanged = Server.MapPath(imgNameNew);

                                    // Kiểm tra tồn tại và xóa file cũ
                                    if (System.IO.File.Exists(imgNameChanged)) System.IO.File.Delete(imgNameChanged);

                                    // Copy thành file hình ảnh mới theo tiêu chí tên hình ảnh SEO
                                    System.IO.File.Copy(imgSrcPath, imgNameChanged);
                                    System.IO.File.Delete(imgSrcPath);

                                    // Gán lại đường dẫn mới (src, alt, title) cho file ảnh
                                    imgConvert = match.Value.Replace(imgSrc, imgNameNew)
                                        .Replace(mAlt.Value != "" ? mAlt.Value : "alt=\"\"", string.Format("alt=\"{0}\"", Link.Replace("-", " ") + " hinh anh " + index))
                                        .Replace(mTitle.Value != "" ? mTitle.Value : "title=\"\"", string.Format("title=\"{0}\"", TitleOrName + " hình ảnh " + index));

                                    strContent = strContent.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1} hình ảnh {2}\">{3}</a>", imgNameNew, TitleOrName, index, imgConvert));
                                //}
                            }
                        }

                    }
                }

                return strContent;
            }
            catch (Exception ex)
            {
                //return strContentInput + string.Empty;
                return ex.Message;
            }
        }
        public string ChangeImageSEO1(object strContentInput, string TitleOrName, string Link)
        {
            try
            {
                int index = 3;
                string strContent = HttpUtility.HtmlDecode(strContentInput + string.Empty);

                // Kiểm tra tồn tại hình ảnh cần xử lý
                if (strContent.ToLower().IndexOf(".jpg") > 0 || strContent.ToLower().IndexOf(".png") > 0 || strContent.ToLower().IndexOf(".gif") > 0 || strContent.ToLower().IndexOf(".jpeg") > 0)
                {
                    // Xóa tất cả các thẻ A-Href bao ngoài thẻ IMG trước khi xử lý
                    strContent = Regex.Replace(strContent, "<a (.*?)><img (.*?)/></a>", "<img $2/>", RegexOptions.IgnoreCase);

                    // Duyệt tất cả các thẻ IMG cần thêm thẻ A-Href
                    foreach (Match match in Regex.Matches(strContent, "<img (.*?) />", RegexOptions.IgnoreCase))
                    {
                        // Lấy đường dẫn của file IMG
                        Match mSrc = Regex.Match(match.Value, "src=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match mAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match mTitle = Regex.Match(match.Value, "title=\"(.*?)\"");

                        string imgSrc = mSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn", "").Replace("http://beautygarden.vn", "").Replace("\"", "");

                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcPath = Server.MapPath(imgSrc);

                            // Kiểm tra tồn tại file hình ảnh trên hoting này ko?
                            if (System.IO.File.Exists(imgSrcPath))
                            {
                                index++;
                                System.Drawing.Image img = System.Drawing.Image.FromFile(imgSrcPath);

                                // Chỉ tạo link về chính hình ảnh hiện tại cho những hình ảnh lớn và thỏa điều kiện
                                //if (img.Width >= 100 && img.Height >= 50)
                                //{
                                    img.Dispose();
                                    string imgName = "", imgPath = "", extName = ".jpg", imgNameNew = "", imgNameChanged = "", imgConvert = "";
                                    string[] arr = imgSrc.Split('/');
                                    imgName = arr[arr.Length - 1];
                                    imgPath = imgSrc.Replace(imgName, "");
                                    arr = imgName.Split('.');
                                    extName = arr[arr.Length - 1];
                                    imgNameNew = string.Format("{0}{1}-hinh-anh-{2}.{3}", imgPath, Link, index, extName);
                                    imgNameChanged = Server.MapPath(imgNameNew);

                                    // Kiểm tra tồn tại và xóa file cũ
                                    if (System.IO.File.Exists(imgNameChanged)) System.IO.File.Delete(imgNameChanged);

                                    // Copy thành file hình ảnh mới theo tiêu chí tên hình ảnh SEO
                                    System.IO.File.Copy(imgSrcPath, imgNameChanged);
                                    System.IO.File.Delete(imgSrcPath);

                                    // Gán lại đường dẫn mới (src, alt, title) cho file ảnh
                                    imgConvert = match.Value.Replace(imgSrc, imgNameNew)
                                        .Replace(mAlt.Value != "" ? mAlt.Value : "alt=\"\"", string.Format("alt=\"{0}\"", Link.Replace("-", " ") + " hinh anh " + index))
                                        .Replace(mTitle.Value != "" ? mTitle.Value : "title=\"\"", string.Format("title=\"{0}\"", TitleOrName + " hình ảnh " + index));

                                    strContent = strContent.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1} hình ảnh {2}\">{3}</a>", imgNameNew, TitleOrName, index, imgConvert));
                                //}
                            }
                        }

                    }
                }

                return strContent;
            }
            catch (Exception ex)
            {
                return strContentInput + string.Empty;
            }
        }
        public string ChangeImageSEO2(object strContentInput, string TitleOrName, string Link)
        {
            try
            {
                int index = 14;
                string strContent = HttpUtility.HtmlDecode(strContentInput + string.Empty);

                // Kiểm tra tồn tại hình ảnh cần xử lý
                if (strContent.ToLower().IndexOf(".jpg") > 0 || strContent.ToLower().IndexOf(".png") > 0 || strContent.ToLower().IndexOf(".gif") > 0 || strContent.ToLower().IndexOf(".jpeg") > 0)
                {
                    // Xóa tất cả các thẻ A-Href bao ngoài thẻ IMG trước khi xử lý
                    strContent = Regex.Replace(strContent, "<a (.*?)><img (.*?)/></a>", "<img $2/>", RegexOptions.IgnoreCase);

                    // Duyệt tất cả các thẻ IMG cần thêm thẻ A-Href
                    foreach (Match match in Regex.Matches(strContent, "<img (.*?) />", RegexOptions.IgnoreCase))
                    {
                        // Lấy đường dẫn của file IMG
                        Match mSrc = Regex.Match(match.Value, "src=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match mAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match mTitle = Regex.Match(match.Value, "title=\"(.*?)\"");

                        string imgSrc = mSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn", "").Replace("http://beautygarden.vn", "").Replace("\"", "");

                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcPath = Server.MapPath(imgSrc);

                            // Kiểm tra tồn tại file hình ảnh trên hoting này ko?
                            if (System.IO.File.Exists(imgSrcPath))
                            {
                                index++;
                                System.Drawing.Image img = System.Drawing.Image.FromFile(imgSrcPath);

                                // Chỉ tạo link về chính hình ảnh hiện tại cho những hình ảnh lớn và thỏa điều kiện
                                //if (img.Width >= 100 && img.Height >= 50)
                                //{
                                    img.Dispose();
                                    string imgName = "", imgPath = "", extName = ".jpg", imgNameNew = "", imgNameChanged = "", imgConvert = "";
                                    string[] arr = imgSrc.Split('/');
                                    imgName = arr[arr.Length - 1];
                                    imgPath = imgSrc.Replace(imgName, "");
                                    arr = imgName.Split('.');
                                    extName = arr[arr.Length - 1];
                                    imgNameNew = string.Format("{0}{1}-hinh-anh-{2}.{3}", imgPath, Link, index, extName);
                                    imgNameChanged = Server.MapPath(imgNameNew);

                                    // Kiểm tra tồn tại và xóa file cũ
                                    if (System.IO.File.Exists(imgNameChanged)) System.IO.File.Delete(imgNameChanged);

                                    // Copy thành file hình ảnh mới theo tiêu chí tên hình ảnh SEO
                                    System.IO.File.Copy(imgSrcPath, imgNameChanged);
                                    System.IO.File.Delete(imgSrcPath);

                                    // Gán lại đường dẫn mới (src, alt, title) cho file ảnh
                                    imgConvert = match.Value.Replace(imgSrc, imgNameNew)
                                        .Replace(mAlt.Value != "" ? mAlt.Value : "alt=\"\"", string.Format("alt=\"{0}\"", Link.Replace("-", " ") + " hinh anh " + index))
                                        .Replace(mTitle.Value != "" ? mTitle.Value : "title=\"\"", string.Format("title=\"{0}\"", TitleOrName + " hình ảnh " + index));

                                    strContent = strContent.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1} hình ảnh {2}\">{3}</a>", imgNameNew, TitleOrName, index, imgConvert));
                               // }
                            }
                        }

                    }
                }

                return strContent;
            }
            catch (Exception ex)
            {
                return strContentInput + string.Empty;
            }
        }
        public string ChangeImageSEO3(object strContentInput, string TitleOrName, string Link)
        {
            try
            {
                int index = 25;
                string strContent = HttpUtility.HtmlDecode(strContentInput + string.Empty);

                // Kiểm tra tồn tại hình ảnh cần xử lý
                if (strContent.ToLower().IndexOf(".jpg") > 0 || strContent.ToLower().IndexOf(".png") > 0 || strContent.ToLower().IndexOf(".gif") > 0 || strContent.ToLower().IndexOf(".jpeg") > 0)
                {
                    // Xóa tất cả các thẻ A-Href bao ngoài thẻ IMG trước khi xử lý
                    strContent = Regex.Replace(strContent, "<a (.*?)><img (.*?)/></a>", "<img $2/>", RegexOptions.IgnoreCase);

                    // Duyệt tất cả các thẻ IMG cần thêm thẻ A-Href
                    foreach (Match match in Regex.Matches(strContent, "<img (.*?) />", RegexOptions.IgnoreCase))
                    {
                        // Lấy đường dẫn của file IMG
                        Match mSrc = Regex.Match(match.Value, "src=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match mAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match mTitle = Regex.Match(match.Value, "title=\"(.*?)\"");

                        string imgSrc = mSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn", "").Replace("http://beautygarden.vn", "").Replace("\"", "");

                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcPath = Server.MapPath(imgSrc);

                            // Kiểm tra tồn tại file hình ảnh trên hoting này ko?
                            if (System.IO.File.Exists(imgSrcPath))
                            {
                                index++;
                                System.Drawing.Image img = System.Drawing.Image.FromFile(imgSrcPath);

                                // Chỉ tạo link về chính hình ảnh hiện tại cho những hình ảnh lớn và thỏa điều kiện
                                //if (img.Width >= 100 && img.Height >= 50)
                                //{
                                    img.Dispose();
                                    string imgName = "", imgPath = "", extName = ".jpg", imgNameNew = "", imgNameChanged = "", imgConvert = "";
                                    string[] arr = imgSrc.Split('/');
                                    imgName = arr[arr.Length - 1];
                                    imgPath = imgSrc.Replace(imgName, "");
                                    arr = imgName.Split('.');
                                    extName = arr[arr.Length - 1];
                                    imgNameNew = string.Format("{0}{1}-hinh-anh-{2}.{3}", imgPath, Link, index, extName);
                                    imgNameChanged = Server.MapPath(imgNameNew);

                                    // Kiểm tra tồn tại và xóa file cũ
                                    if (System.IO.File.Exists(imgNameChanged)) System.IO.File.Delete(imgNameChanged);

                                    // Copy thành file hình ảnh mới theo tiêu chí tên hình ảnh SEO
                                    System.IO.File.Copy(imgSrcPath, imgNameChanged);
                                    System.IO.File.Delete(imgSrcPath);

                                    // Gán lại đường dẫn mới (src, alt, title) cho file ảnh
                                    imgConvert = match.Value.Replace(imgSrc, imgNameNew)
                                        .Replace(mAlt.Value != "" ? mAlt.Value : "alt=\"\"", string.Format("alt=\"{0}\"", Link.Replace("-", " ") + " hinh anh " + index))
                                        .Replace(mTitle.Value != "" ? mTitle.Value : "title=\"\"", string.Format("title=\"{0}\"", TitleOrName + " hình ảnh " + index));

                                    strContent = strContent.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1} hình ảnh {2}\">{3}</a>", imgNameNew, TitleOrName, index, imgConvert));
                                //}
                            }
                        }

                    }
                }

                return strContent;
            }
            catch (Exception ex)
            {
                return strContentInput + string.Empty;
            }
        }
        public string ChangeImageSEO4(object strContentInput, string TitleOrName, string Link)
        {
            try
            {
                int index = 35;
                string strContent = HttpUtility.HtmlDecode(strContentInput + string.Empty);

                // Kiểm tra tồn tại hình ảnh cần xử lý
                if (strContent.ToLower().IndexOf(".jpg") > 0 || strContent.ToLower().IndexOf(".png") > 0 || strContent.ToLower().IndexOf(".gif") > 0 || strContent.ToLower().IndexOf(".jpeg") > 0)
                {
                    // Xóa tất cả các thẻ A-Href bao ngoài thẻ IMG trước khi xử lý
                    strContent = Regex.Replace(strContent, "<a (.*?)><img (.*?)/></a>", "<img $2/>", RegexOptions.IgnoreCase);

                    // Duyệt tất cả các thẻ IMG cần thêm thẻ A-Href
                    foreach (Match match in Regex.Matches(strContent, "<img (.*?) />", RegexOptions.IgnoreCase))
                    {
                        // Lấy đường dẫn của file IMG
                        Match mSrc = Regex.Match(match.Value, "src=\"(.*?)\"", RegexOptions.IgnoreCase);
                        Match mAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match mTitle = Regex.Match(match.Value, "title=\"(.*?)\"");

                        string imgSrc = mSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn", "").Replace("http://beautygarden.vn", "").Replace("\"", "");

                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcPath = Server.MapPath(imgSrc);

                            // Kiểm tra tồn tại file hình ảnh trên hoting này ko?
                            if (System.IO.File.Exists(imgSrcPath))
                            {
                                index++;
                                System.Drawing.Image img = System.Drawing.Image.FromFile(imgSrcPath);

                                // Chỉ tạo link về chính hình ảnh hiện tại cho những hình ảnh lớn và thỏa điều kiện
                                //if (img.Width >= 100 && img.Height >= 50)
                                //{
                                    img.Dispose();
                                    string imgName = "", imgPath = "", extName = ".jpg", imgNameNew = "", imgNameChanged = "", imgConvert = "";
                                    string[] arr = imgSrc.Split('/');
                                    imgName = arr[arr.Length - 1];
                                    imgPath = imgSrc.Replace(imgName, "");
                                    arr = imgName.Split('.');
                                    extName = arr[arr.Length - 1];
                                    imgNameNew = string.Format("{0}{1}-hinh-anh-{2}.{3}", imgPath, Link, index, extName);
                                    imgNameChanged = Server.MapPath(imgNameNew);

                                    // Kiểm tra tồn tại và xóa file cũ
                                    if (System.IO.File.Exists(imgNameChanged)) System.IO.File.Delete(imgNameChanged);

                                    // Copy thành file hình ảnh mới theo tiêu chí tên hình ảnh SEO
                                    System.IO.File.Copy(imgSrcPath, imgNameChanged);
                                    System.IO.File.Delete(imgSrcPath);

                                    // Gán lại đường dẫn mới (src, alt, title) cho file ảnh
                                    imgConvert = match.Value.Replace(imgSrc, imgNameNew)
                                        .Replace(mAlt.Value != "" ? mAlt.Value : "alt=\"\"", string.Format("alt=\"{0}\"", Link.Replace("-", " ") + " hinh anh " + index))
                                        .Replace(mTitle.Value != "" ? mTitle.Value : "title=\"\"", string.Format("title=\"{0}\"", TitleOrName + " hình ảnh " + index));

                                    strContent = strContent.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1} hình ảnh {2}\">{3}</a>", imgNameNew, TitleOrName, index, imgConvert));
                                //}
                            }
                        }

                    }
                }

                return strContent;
            }
            catch (Exception ex)
            {
                return strContentInput + string.Empty;
            }
        }
    }
}
