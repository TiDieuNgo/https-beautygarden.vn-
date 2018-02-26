using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class CayMenuModel
    {
        public IList<Menu> DanhMucs { get; set; }
        public int SlSanPhamHienTai { get; set; }
        public int Sldudieukien { get; set; }
    }

    public class CayDanhMucItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Q1 { get; set; }
        public int Q2 { get; set; }

        public IList<int> IdsCon { get; set; } 

        public IList<CayDanhMucItemModel> Childs { get; set; }

        public CayDanhMucItemModel()
        {
            IdsCon=new List<int>();
            Childs=new List<CayDanhMucItemModel>();
        }
    }
}