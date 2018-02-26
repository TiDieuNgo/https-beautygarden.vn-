using System.Collections.Generic;

namespace Shop.Web.Core.Models
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public const string Phan_Quyen_View = "1";
        public const string Phan_Quyen_Update = "2";

        public const string Danh_Muc_View = "3";
        public const string Danh_Muc_Edit = "4";
        public const string Danh_Muc_Delete = "5";
        public const string Danh_Muc_Create = "26";

        public const string San_Pham_View = "6";
        public const string San_Pham_Edit = "7";
        public const string San_Pham_Delete = "8";
        public const string San_Pham_Create = "27";

        public const string Khuyen_mai_View = "9";

        public const string Banner_Create = "10";
        public const string Banner_Edit = "11";
        public const string Banner_Delete = "12";

        public const string Tintuc_Create = "13";
        public const string Tintuc_Edit = "14";
        public const string Tintuc_Delete = "15";

        public const string Thuong_hieu_Create = "16";
        public const string Thuong_hieu_Edit = "17";
        public const string Thuong_hieu_Delete = "18";

        public const string Ykienkhachhang_Create = "19";
        public const string Ykienkhachhang_Edit = "20";
        public const string Ykienkhachhang_Delete = "21";

        public const string User_View = "22";
        public const string User_Edit = "23";
        public const string User_Delete = "24";
        public const string User_Create = "25";

        public const string redirect_create = "28";
        public const string redirect_Edit = "29";

        public const string ThongBaoKhiCoHang_View = "30";

        public static IList<RoleList> GetRoles()
        {
            IList<RoleList> roleLists = new List<RoleList>(0);
            #region [phan quyen]
            //xem phan quyen
            roleLists.Add(new RoleList()
            {
                Name = "Xem quyền",
                Id = int.Parse(Phan_Quyen_View)
            });
            //update phan quyen
            roleLists.Add(new RoleList()
            {
                Name = "Update quyền",
                Id = int.Parse(Phan_Quyen_Update)
            });

            #endregion
            #region [danh muc]
            //----------------xem danh muc---------------
            roleLists.Add(new RoleList()
            {
                Name = "Xem danh mục",
                Id = int.Parse(Danh_Muc_View)
            });
            //sửa danh muc
            roleLists.Add(new RoleList()
            {
                Name = "Sửa danh mục",
                Id = int.Parse(Danh_Muc_Edit)
            });
            //Xóa danh muc
            roleLists.Add(new RoleList()
            {
                Name = "Xóa danh mục",
                Id = int.Parse(Danh_Muc_Delete)
            });
            //thêm danh muc
            roleLists.Add(new RoleList()
            {
                Name = "Thêm danh mục",
                Id = int.Parse(Danh_Muc_Create)
            });
            #endregion
            #region [san pham]
            //xem sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Xem sản phẩm",
                Id = int.Parse(San_Pham_View)
            });
            //sửa sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Sửa sản phẩm",
                Id = int.Parse(San_Pham_Edit)
            });
            //Xóa sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Xóa sản phẩm",
                Id = int.Parse(San_Pham_Delete)
            });
            //thêm sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Thêm sản phẩm",
                Id = int.Parse(San_Pham_Create)
            });
            #endregion
            #region [khuye mai]
            //Xem khuyến mãi
            roleLists.Add(new RoleList()
            {
                Name = "Xem khuyến mãi",
                Id = int.Parse(Khuyen_mai_View)
            });
            #endregion
            #region [banner]
            //thêm mới banner
            roleLists.Add(new RoleList()
            {
                Name = "Thêm mới banner",
                Id = int.Parse(Banner_Create)
            });
            //sửa sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Sửa banner",
                Id = int.Parse(Banner_Edit)
            });
            //Xóa sản phẩm
            roleLists.Add(new RoleList()
            {
                Name = "Xóa banner",
                Id = int.Parse(Banner_Delete)
            });
            #endregion
            #region [tin tuc]
            //xem tin tức
            roleLists.Add(new RoleList()
            {
                Name = "Xem tin tức",
                Id = int.Parse(Tintuc_Create)
            });
            //sửa tin tức
            roleLists.Add(new RoleList()
            {
                Name = "Sửa tin tức",
                Id = int.Parse(Tintuc_Edit)
            });
            //Xóa tin tức
            roleLists.Add(new RoleList()
            {
                Name = "Xóa tin tức",
                Id = int.Parse(Tintuc_Delete)
            });
            #endregion
            #region [thuong hieu]
            //thêm thương hiệu
            roleLists.Add(new RoleList()
            {
                Name = "Thêm thương hiệu",
                Id = int.Parse(Thuong_hieu_Create)
            });
            //sửa thương hiệu
            roleLists.Add(new RoleList()
            {
                Name = "Sửa thương hiệu",
                Id = int.Parse(Thuong_hieu_Edit)
            });
            //Xóa thương hiệu
            roleLists.Add(new RoleList()
            {
                Name = "Xóa thương hiệu",
                Id = int.Parse(Thuong_hieu_Delete)
            });
            #endregion
            #region [y kien khach hang]
            //Thêm ý kiến khách hàng
            roleLists.Add(new RoleList()
            {
                Name = "Thêm ý kiến khách hàng",
                Id = int.Parse(Ykienkhachhang_Create)
            });
            //sửa ý kiến khách hàng
            roleLists.Add(new RoleList()
            {
                Name = "Sửa ý kiến khách hàng",
                Id = int.Parse(Ykienkhachhang_Edit)
            });
            //Xóa ý kiến khách hàng
            roleLists.Add(new RoleList()
            {
                Name = "Xóa ý kiến khách hàng",
                Id = int.Parse(Ykienkhachhang_Delete)
            });
            #endregion
            #region [User]
            //xem User
            roleLists.Add(new RoleList()
            {
                Name = "Xem User",
                Id = int.Parse(User_View)
            });
            //sửa User
            roleLists.Add(new RoleList()
            {
                Name = "Sửa User",
                Id = int.Parse(User_Edit)
            });
            //Xóa User
            roleLists.Add(new RoleList()
            {
                Name = "Xóa User",
                Id = int.Parse(User_Delete)
            });
            //tao User
            roleLists.Add(new RoleList()
            {
                Name = "Tạo User",
                Id = int.Parse(User_Create)
            });
            #endregion
            #region [Redirect]
            //tao Redirect
            roleLists.Add(new RoleList()
                          {
                              Name = "Tạo redirect 301",
                              Id = int.Parse(redirect_create)
                          });
            //update Redirect
            roleLists.Add(new RoleList()
                          {
                              Name = "Update redirect 301",
                              Id = int.Parse(redirect_Edit)
                          });

            #endregion
            #region [thông báo khi có hàng]
            //xem
            roleLists.Add(new RoleList()
                          {
                              Name = "Xem thông báo khi có hàng",
                              Id = int.Parse(ThongBaoKhiCoHang_View)
                          });
            #endregion
            return roleLists;
        }
    }
    public class RoleList
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public enum UserRoles
    {
        Admin = 1,
        User = 2       
    }
}
