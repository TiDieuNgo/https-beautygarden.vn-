using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class TagsLinkRepository : RepositoryBase<TagsLink>, ITagsLinkRepository
    {
        public TagsLinkRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IList<TagMapping> GetTenSanPhamInMenu()
        {
            return (from tintuc in DataContext.TagsLink
                    join danhmuctintuc in DataContext.Menu on tintuc.IdMenu equals danhmuctintuc.id_
                    //where tintuc.IdMenu == id
                    select new TagMapping()
                    {
                        idMenu = tintuc.IdMenu,
                        Link = tintuc.Link,
                        TenTags = tintuc.TenTags,
                        id_ = tintuc.id_,
                        TenSanPham = danhmuctintuc.NameProduct
                    }).ToList();
        }
        public TagMapping GetTagLinkById(int id)
        {
            return (from tintuc in DataContext.TagsLink
                    join danhmuctintuc in DataContext.Menu on tintuc.IdMenu equals danhmuctintuc.id_
                    where tintuc.IdMenu == id
                    select new TagMapping()
                    {
                        idMenu = tintuc.IdMenu,
                        Link = tintuc.Link,
                        TenTags = tintuc.TenTags,
                        id_ = tintuc.id_,
                        TenSanPham = danhmuctintuc.NameProduct
                    }).First();
        }
    }
    public interface ITagsLinkRepository : IRepository<TagsLink>
    {
        IList<TagMapping> GetTenSanPhamInMenu();
        TagMapping GetTagLinkById(int id);
    }
}
