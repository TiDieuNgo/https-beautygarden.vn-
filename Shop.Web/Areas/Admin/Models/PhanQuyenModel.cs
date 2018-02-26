using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Areas.Admin.Models
{
    public class PhanQuyenModel
    {
        public class QuyenModel
        {
            public string TenNhanVien { get; set; }
            public string Quyen { get; set; }
            public int NhanVienId { get; set; }
        }
        public class SetQuyen
        {
            public int NhanVienId { get; set; }
            public string TenQuyen { get; set; }
            public bool Selected { get; set; }
            public int IdQuyen { get; set; }
        }
        public class SaveModel
        {
            public int idNhanVien { get; set; }
            public int IdQuyen { get; set; }
            public bool Checked { get; set; }
        }
    }
}