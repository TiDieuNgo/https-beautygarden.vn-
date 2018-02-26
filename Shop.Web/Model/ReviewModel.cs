using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Model
{
    public class ReviewModel
    {
        public PagedList.IPagedList<Review> Reviews { get; set; }
        public IList<Review> Reviewkhacs { get; set; }
        public Review Review { get; set; }
        public IList<TagTinTuc> TagTinTucs { get; set; }
    }
}