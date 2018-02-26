using System.Collections.Generic;
using Shop.Model;

namespace Shop.Web.Model
{
    public class TraLoiCommentModel
    {
        public IList<TraLoiComment> TraLoiComments { get; set; }
        public TraLoiComment TraLoiComment { get; set; }
        public Comment Comment { get; set; }
        public IList<Comment> CauHoiKhacs { get; set; } 
    }
}