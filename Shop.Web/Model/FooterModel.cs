using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Models
{
    public class FooterModel
    {
        public IList<Menu> VeChungTois { get; set; }
        public IList<Menu> HoTros { get; set; }
        public IList<TuKhoaTimKiem> TuKhoaTimKiems { get; set; }
        public CauHinh CauHinh { get; set; }
    }
}