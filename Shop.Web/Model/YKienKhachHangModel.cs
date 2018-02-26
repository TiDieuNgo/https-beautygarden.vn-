using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Model
{
    public class YKienKhachHangModel
    {

        public IList<YKienKhachHang> YKienKhachHangs { get; set; }
        public YKienKhachHang Ykien { get; set; }

        public static List<TuKhoaTimKiem> GetListTop10()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<TuKhoaTimKiem>("select top 10 * from TuKhoaTimKiems order by CountRow desc").ToList();
                return data;
            }
        }

        public static List<Menu> GetproductIdsList()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>("select * from menu where HasValue=1 and ok=1 and HasOnHand=1").ToList();
                return data;
            }
        }
        public static List<Menu> GetAllLinkforsitemap()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>("select * from menu where ok=1 and id_ not in(19,21,23,24,25,27,31,32,59,60,61)").ToList();
                return data;
            }
        }
        public static List<Menu> GetThuongHieuList()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>("select * from Menu where Menu.style='thuong-hieu' and idControl<>0 and ok=1").ToList();
                return data;
            }
        }
        public static List<Menu> GetDanhMucList()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>("select * from Menu where Menu.style='danh-muc-san-pham' and idControl<>0 and ok=1 and ShowMenuTop=1").ToList();
                return data;
            }
        }
        public static List<DetailMenu> GetchitietSKKMs()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<DetailMenu>("select * from DetailMenu where DetailMenu.id_menu=20 and ok=1 and ShowMenu=1 and TinhTrangSP=1 and ShowKhuyenMai=0").ToList();
                return data;
            }
        }
        public static List<DetailMenu> GetchitietBQLDs()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<DetailMenu>("select * from DetailMenu where DetailMenu.id_menu=20 and ok=1 and ShowMenu=1 and TinhTrangSP=0 and ShowKhuyenMai=1").ToList();
                return data;
            }
        }
        public static List<Review> GetchitietReviewSPs()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Review>("select * from Reviews").ToList();
                return data;
            }
        }
        public static List<Redirect301> GetLinkForredirect()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Redirect301>("select * from redirect301s ").ToList();
                return data;
            }
        }
        public static void TukhoaTimkiemAdd(string tukhoa)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<TuKhoaTimKiem>("SELECT TOP 1 * FROM TuKhoaTimKiems WHERE TuKhoaKhongDau COLLATE Latin1_General_CI_AI LIKE N'%" + RejectMarks(tukhoa) + "%'").FirstOrDefault();
                if (data == null)
                {
                    SqlParameter TuKhoa = new SqlParameter("TuKhoa", tukhoa);
                    SqlParameter TuKhoaKhongDau = new SqlParameter("TuKhoaKhongDau", RejectMarks(tukhoa));

                    int? newIdentityValue = context.Database.SqlQuery<Int32>("addTuKhoaTimKiems @TuKhoa, @TuKhoaKhongDau", TuKhoa, TuKhoaKhongDau).FirstOrDefault();
                    context.Database.ExecuteSqlCommand("insert into TuKhoaTapHops (IdTuKhoa,TuKhoa, TuKhoaKhongDau,CreatedAt) values (" + newIdentityValue + ", N'" + tukhoa + "', N'" + RejectMarks(tukhoa) + "', GETDATE())");
                }
                else
                {
                    context.Database.ExecuteSqlCommand("insert into TuKhoaTapHops (IdTuKhoa,TuKhoa, TuKhoaKhongDau,CreatedAt) values (" + data.Id + ", N'" + tukhoa + "', N'" + RejectMarks(tukhoa) + "', GETDATE())");
                    var dataCount = context.Database.SqlQuery<TuKhoaTimKiem>("SELECT * FROM TuKhoaTapHops WHERE IdTuKhoa = " + data.Id + " ");
                    context.Database.ExecuteSqlCommand("UPDATE TuKhoaTimKiems Set CountRow = " + dataCount.Count() + " WHERE Id = " + data.Id + " ");
                }
            }
        }
        public static string GetContentbylink(string splink)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select Note from Menu where Menu.Link='{0}'", splink);
                return context.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
        }
        public static Menu GetProductbyLink(string splink)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select * from Menu where Menu.Link='{0}'", splink);
                return context.Database.SqlQuery<Menu>(sql).FirstOrDefault();
            }
        }
        public static Menu GetMenuByBarcode(string barcode)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select * from Menu where id_ = (select IdMenu from MenuOption where Barcode='{0}')",barcode);
                return context.Database.SqlQuery<Menu>(sql).FirstOrDefault();
            }
        }
        public static double GetRatingbyLink(string splink)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("SELECT ISNULL(Round(SUM(Rating)/COUNT(Id), 1),1) FROM UserRatings where IdSanPham = (select id_ from Menu where Link='{0}')", splink);
                return context.Database.SqlQuery<double>(sql).First();
            }
        }
        public static int GetRatingCountbyLink(string splink)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("SELECT COUNT(*) FROM UserRatings where IdSanPham = (select id_ from Menu where Link='{0}')", splink);
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
        public static string GetDanhMucHienTai(int idsanpham)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select NameProduct from Menu where id_ in (select idMenuCatelogy from MenuProAdd where idMenuProAdded={0})", idsanpham).FirstOrDefault();
                return data;
            }
        }
        public static string GetDanhMucCap1(int idsanpham)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select NameProduct from Menu where id_=(select top 1 idControl from Menu where id_ in (select idMenuCatelogy from MenuProAdd where idMenuProAdded={0}))", idsanpham).FirstOrDefault();
                return data;
            }
        }
        public static IList<MenuProAdd> GetSPThuocDMLevel3()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<MenuProAdd>(@"select * from MenuProAdd where idMenuCatelogy 
                                                                            in  (select id_ from Menu where idControl 
                                                                            in (select  id_ from Menu where idControl 
                                                                            in (select id_ from Menu where idControl=1 and ok =1)))").ToList();
                return data;
            }
        }
        public static string Gettendanhmucconcap3(int iddanhmuc)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select NameProduct from Menu where Menu.id_ ={0}", iddanhmuc).FirstOrDefault();
                return data;
            }
        }
        public static string GetTenSanPhamTheoDanhMuccon3(int idSP)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select NameProduct from Menu where Menu.id_ ={0}", idSP).FirstOrDefault();
                return data;
            }
        }

        public static string GetLinkTheoDanhMuccon3(int idSP)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select Link from Menu where Menu.id_ ={0}", idSP).FirstOrDefault();
                return data;
            }
        }
        public static IList<Menu> GetDataforZalo()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>(@"select * from Menu where idcontrol=11 
                                                                and HasValue=1 and HasOnHand=1 and DungSai=1 and TenNgan is not null 
                                                                and MoTaNgan is not null and sDate BETWEEN '2017-07-18 17:00:00.000' and '2017-08-08 08:24:22.222'").ToList();
                return data;
            }
        }
        public static IList<Menu> GetDataforFacebook()
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<Menu>(@"select * from Menu where id_ in (select IdMenu from ProductStockSync 
															where IdMenu in (select id_
                                                            from Menu
                                                            where idControl = 11
                                                            and MoTaNgan is not null 
                                                            and TenNgan is not null
															and HasOnHand = 1 
															and HasValue =1 
															and ok =1)
															and OnHand >=3)").ToList();
                return data;
            }
        }
        public static string GetThuongHieuForFacebook(int idSP)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select  NameProduct from Menu where Menu.id_ = (select IdNhaCungCap from Menu where id_ = {0})", idSP).FirstOrDefault();
                return data;
            }
        }
        public static string GetDanhMucForFacebook(int idSP)
        {
            using (var context = new ShopDataContex())
            {
                var data = context.Database.SqlQuery<string>("select NameProduct from Menu where Menu.id_ = (select top 1 idMenuCatelogy from MenuProAdd where idMenuProAdded = {0})", idSP).FirstOrDefault();
                return data;
            }
        }
        public static string RejectMarks(string text)
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
    }
}