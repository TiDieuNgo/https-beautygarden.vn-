using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;

namespace Shop.Data.Repositories
{
    public class MenuImageRepository : RepositoryBase<MenuImage>, IMenuImageRepository
    {
        public MenuImageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IList<MenuImageMapping.MenuImage> GetImagesByMenuId(int mennuId)
        {
            return (from menuImage in DataContext.MenuImage
                    where menuImage.idMenu == mennuId
                    select new MenuImageMapping.MenuImage()
                               {
                                   id = menuImage.id,
                                   ImageName = menuImage.ImageName
                               }).ToList();
        }
        public IList<MenuImage> GetImagesKhacs(int id)
        {
            string sql = string.Format("select * from MenuImage where MenuImage.idMenu={0}",id);
            return this.DataContext.Database.SqlQuery<MenuImage>(sql).ToList();
        }
    }
    public interface IMenuImageRepository : IRepository<MenuImage>
    {
        IList<MenuImageMapping.MenuImage> GetImagesByMenuId(int mennuId);
        IList<MenuImage> GetImagesKhacs(int id);
    }
}
