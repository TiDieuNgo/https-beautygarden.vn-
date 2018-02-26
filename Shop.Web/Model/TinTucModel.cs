using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Model
{
    public class TinTucModel
    {
        public PagedList.IPagedList<DetailMenu> TinTucs { get; set; }
        public TinTucMapping TinTuc { get; set; }
        public IList<DetailMenu> tinkhacs { get; set; }
        public DetailMenu TinTucChiTiet { get; set; }
        public string ContentDatasrc { get; set; }
        public IList<TagTinTuc> TagTinTucs { get; set; }
        public IList<DetailMenu> TagForTinTucs { get; set; }
        public TagTinTuc TagTinTuc { get; set; }
    }
}