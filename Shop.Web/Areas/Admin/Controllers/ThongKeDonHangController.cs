using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class ThongKeDonHangController : BaseController
    {
        private readonly IDetailMenuCommentRepository _menuCommentRepository;
        private readonly IDetailMenuCommentItemRepository _detailMenuCommentItemRepository;
        private readonly IMenuRepository _menuRepository;
        private const int pageSize = 50;
        public ThongKeDonHangController(IDetailMenuCommentRepository menuCommentRepository, IDetailMenuCommentItemRepository detailMenuCommentItemRepository, IMenuRepository menuRepository)
        {
            _menuCommentRepository = menuCommentRepository;
            _detailMenuCommentItemRepository = detailMenuCommentItemRepository;
            _menuRepository = menuRepository;
        }

        public ActionResult Index(string from, string to, int page = 1)
       {
               int pageNumber = page;
               DateTime start = !string.IsNullOrEmpty(from) ? ConvertToShortDay(from) : DateTime.Today;   //datetime.today.adds(-15)
               DateTime end = !string.IsNullOrEmpty(to) ? ConvertToShortDay(to) : DateTime.Today;
               DateTime startday = StartOfDay(start);
               DateTime endDay = EndOfDay(end);
               IList<DetailMenuCommentItem> hoaDons =
              _detailMenuCommentItemRepository.GetMany(o => o.sDate >= start && o.sDate <= endDay).ToList();

               IList<GroupModel> groupsv2 = hoaDons.Any()
                                             ? hoaDons.GroupBy(o => o.Name).
                                                   Select(
                                                       o => new GroupModel
                                                       {
                                                           Count = o.Count(),
                                                           Name = o.Key
                                                       }).ToList()
                                             : new List<GroupModel>();

                var sortdonhangs = groupsv2.OrderByDescending(o => o.Count);
               DonHangModel model=new DonHangModel()
                                      {
                                          From = start.ToString("dd/MM/yyyy"),
                                          To = endDay.ToString("dd/MM/yyyy"),
                                          danhsachsanphams = sortdonhangs.ToPagedList(pageNumber,pageSize),
                                          page = page,
                                          solanmua = groupsv2.Count()
                                      };
           
           return View("Index", model);
       }
       public DateTime ConvertToShortDay(string input)
       {
           DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
           dtfi.ShortDatePattern = "MM/dd/yyyy HH:mm";
           dtfi.DateSeparator = "/";

           return Convert.ToDateTime(Regex.Replace(input,
               @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4})\b",
               "${month}/${day}/${year}"), dtfi);
       }

       public DateTime StartOfDay(DateTime source)
       {
           return Convert.ToDateTime(source.ToShortDateString());
       }

       public DateTime EndOfDay(DateTime source)
       {
           return Convert.ToDateTime(source.ToShortDateString() + " 11:59 PM");
       }
       public class DonHangModel
       {
           public int solanmua { get; set; }
           public string TenSanPham { get; set; }
           public DateTime ngaymua { get; set; }
           public IPagedList<GroupModel> danhsachsanphams { get; set; }
           public string From { get; set; }
           public string To { get; set; }
           public int stt { get; set; }
           public int page { get; set; } 
       }
       public class GroupModel
       {
           public int Count { get; set; }
           public string Name { get; set; }
       }
        public class  SanPhamModel //brandModel
        {
            public string Nameproduct { get; set; }
            public int soluong { get; set; }
        }
     
    }
}
