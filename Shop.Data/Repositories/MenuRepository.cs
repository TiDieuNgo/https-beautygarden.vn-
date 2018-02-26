using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data.Infrastructure;
using Shop.Model;
using Shop.Web.Core.Cache;
using Shop.Web.Core.Caching;


namespace Shop.Data.Repositories
{
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
       

        private const int time = 24*60*2;

        private readonly ICacheManager _cacheManager;

        public MenuRepository(IDatabaseFactory databaseFactory, ICacheManager cacheManager)
            : base(databaseFactory)
        {
            _cacheManager = cacheManager;
        }

        public IList<Menu> GetAllMenuCache()
        {
            return _cacheManager.Get(CacheKey.AllMenu, time, () =>
                                                                 {
                                                                     return
                                                                         (from menu in DataContext.Menu.AsNoTracking()
                                                                          where
                                                                              menu.Style == "danh-muc-san-pham" ||
                                                                              menu.idControl == 11
                                                                          orderby menu.sPosition
                                                                          select menu
                                                                         ).ToList();
                                                                 });
        }

        public IList<ProductFrontPageMapping.BannerShowHome> GetBannersHome()
        {
            return _cacheManager.Get(CacheKey.BannerHomeCacheKey, time, () =>
                                                                   {
                                                                       return
                                                                           (from menu in DataContext.Menu.AsNoTracking()
                                                                            where
                                                                                menu.idControl == 23 &&
                                                                                menu.Style ==
                                                                                "banner-website-top-desktop" &&
                                                                                menu.ok
                                                                            orderby menu.sPosition
                                                                            select
                                                                                new ProductFrontPageMapping.
                                                                                BannerShowHome
                                                                                    {
                                                                                        Name = menu.NameProduct,
                                                                                        Link = menu.LinkHttp1,
                                                                                        Img = menu.Img
                                                                                    }).ToList();
                                                                   });
        }

        public IList<ProductFrontPageMapping.BrandMapping> GetAllBrandCache()
        {
            return _cacheManager.Get(CacheKey.AllBrandCacheKey, time, () =>
            {
                return
                    (from menu in DataContext.Menu.AsNoTracking()
                     where
                         menu.Style == "thuong-hieu"
                     orderby menu.sPosition
                     select
                         new ProductFrontPageMapping.
                         BrandMapping
                         {
                             Id = menu.id_,
                             Name = menu.NameProduct,
                             Link = menu.Link,
                             Img = menu.Img,
                             DungSai = menu.DungSai,
                             idControl = menu.idControl,
                             ok = menu.ok
                         }).ToList();
            });
        }

        

        public void ClearCacheByKey(IList<string> keys)
        {
            foreach (var key in keys)
            {
                _cacheManager.Remove(key);
            }
           
        }
        public void ClearCacheBannerHome(string key)
        {
                _cacheManager.Remove(key);
        }
        //tim kiem san pham
        public IList<Menu> TimKiem(string key)
        {
            string sql =
                string.Format(@"select 
		                            * 
	                            from Menu
	                             where 
		                            (
		                            LOWER(Menu.NameProduct) LIKE N'%{0}' 
		                            Or LOWER(Menu.NameProduct) LIKE N'{0}%' 
		                            or LOWER(Menu.NameProduct) like N'%{0}%' 
		                            OR LOWER(Menu.NameProductLong) LIKE N'%{0}' 
		                            Or LOWER(Menu.NameProductLong) LIKE N'{0}%' 
		                            or LOWER(Menu.NameProductLong) like N'%{0}%'
		                            OR LOWER(Menu.CodeProduct) LIKE '%{0}' 
		                            Or LOWER(Menu.CodeProduct) LIKE '{0}%' 
		                            or LOWER(Menu.CodeProduct) like '%{0}%'
		                            ) 
		                            and (HasValue=1)", key);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //kiem tra ma vach co ton tai trong he thong hay khong
        public int ChekBarcode(string mavach)
        {
            string sql = string.Format("select COUNT(*) from [bg.hvnet.vn].dbo.KV_SanPham  where Code ='{0}'", mavach);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //kiem tra san pham con hang hay het hang
        public int CheckOnhand(string mavach)
        {
            string sql = string.Format("SELECT SUM(OnHand) FROM ProductStockSync where Barcode='{0}' and Idbranch in(48609,14364,48610,48691,48611)",mavach);
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        //lay id cua san pham de truyen vao 1 so bang khac
        public int GetIdMenu()
        {
            string sql = string.Format("select top 1 id_ from Menu order by id_ desc");
            return this.DataContext.Database.SqlQuery<int>(sql).First();
        }
        public IList<Menu>  TimKiemByTenAndBarcode(string key)
        {
                                                                string sql = string.Format(@"SELECT 
	                                                    * 
                                                    FROM Menu 
	                                                    INNER JOIN MenuOption ON Menu.id_=MenuOption.IdMenu 
                                                    where 
	                                                    LOWER(Menu.NameProduct) LIKE N'%{0}' 
	                                                    Or LOWER(Menu.NameProduct) LIKE N'{0}%' 
	                                                    or LOWER(Menu.NameProduct) like N'%{0}%' 
	                                                    OR LOWER(Menu.NameProductLong) LIKE N'%{0}' 
	                                                    Or LOWER(Menu.NameProductLong) LIKE N'{0}%' 
	                                                    or LOWER(Menu.NameProductLong) like N'%{0}%'
	                                                    OR LOWER(Menu.CodeProduct) LIKE '%{0}' 
	                                                    Or LOWER(Menu.CodeProduct) LIKE '{0}%' 
	                                                    or LOWER(Menu.CodeProduct) like '%{0}%'
	                                                    or LOWER(MenuOption.Barcode) LIKE '%{0}'
	                                                    Or LOWER(MenuOption.Barcode) LIKE '{0}%' 
	                                                    or LOWER(MenuOption.Barcode) like '%{0}%' 
	                                                    and HasValue=1 and HasOnHand=1", key);
			   return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //update hasvalue =1
        public IList<Menu> UpdateHasvalue(int id, bool hasvalue)
        {
            string sql = string.Format("update Menu set HasValue='{0}' where id_ = {1}",hasvalue,id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();

        }
        //update hasOnhand=0 tuc la het hang
        public IList<Menu> UpdateHasOnhand(int id, bool hasOnhand)
        {
            string sql = string.Format("update	Menu set HasOnHand='{0}' where id_ = {1}",hasOnhand,id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay ten danh muc de hien thi
        public string GetTenDanhMuc(string link)
        {
            string sql = string.Format("select NameProduct from menu where Menu.Link='{0}'", link);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //lay ten danh muc cha
        public string GetTenDanhMucCha(string link)
        {
            string sql = string.Format("select NameProduct from menu where Menu.id_=(select idControl from menu where Menu.Link='{0}')", link);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //lay link danh muc cha
        public string GetLinkDanhMucCha(string link)
        {
            string sql = string.Format("select Link from menu where Menu.id_=(select idControl from menu where Menu.Link='{0}')", link);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //get id duong dan de them moi danh muc
        public int GetidDanhMuc(int id)
        {
            string sql = string.Format("select id_ from menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //lay danh sach san pham cung loai (lay theo danh muc)
        public IList<Menu> GetSanPhamCungLoai(int id)
        {
            string sql = string.Format("SELECT  * FROM Menu where id_ in (select idMenuProAdded from MenuProAdd where idMenuCatelogy=(select top 1 idMenuCatelogy from MenuProAdd where idMenuProAdded={0})) and HasValue=1 and HasOnHand=1", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();

        }
        //lay danh sach san pham mua cung (lay theo thuong hieu)
        public IList<Menu> GetSanPhamMuaCung(int id)
        {
            string sql = string.Format("SELECT * from Menu WHERE Menu.IdNhaCungCap=(select IdNhaCungCap from Menu where Menu.id_={0})  and HasValue=1 and HasOnHand=1", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay danh sach san pham khi click vao danh muc 
        public IList<Menu> GetSanPhamTheoDanhMuc(int id)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and HasValue=1 and HasOnHand=1 AND MenuProAdd.idMenuCatelogy ={0}",id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }

        public IList<MenuCategoryForAddPromotionmapping> GetListProductsByCategoryIds(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_

                where ids.Contains(menuProAdd.idMenuCatelogy) && menu.HasValue&&menu.HasOnHand&&menu.ok
                group menu by menu.id_ into gr
                
                select new MenuCategoryForAddPromotionmapping
                {
                    ProductId = gr.Key,
                    
                    Name = gr.FirstOrDefault().NameProduct,
                    Price = gr.FirstOrDefault().PricePro
                }).ToList();
        }

        public IList<MenuCategoryForAddPromotionmapping> GetByIds(IList<int> ids)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                where ids.Contains(menu.id_) && menu.HasValue && menu.HasOnHand && menu.ok && menu.Style.Equals("san-pham")
                    orderby menu.NameProduct
                select new MenuCategoryForAddPromotionmapping
                {
                    ProductId = menu.id_,
                    Name = menu.NameProduct,
                    Price = menu.PricePro
                }).ToList();
        }

        public IList<MenuCategoryForAddPromotionmapping> GetAllProductsForPromotion()
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.HasOnHand && menu.ok && menu.Style.Equals("san-pham")
                    orderby menu.NameProduct
                    select new MenuCategoryForAddPromotionmapping
                    {
                        ProductId = menu.id_,
                        Name = menu.NameProduct,
                        Price = menu.PricePro
                    }).ToList();
        }

        public IList<MenuCategoryForAddPromotionmapping> AutocompleteSearch(string q)
        {
            string sql =
                string.Format(
                    "select id_ as ProductId," +
                    "NameProduct as Name, "+
                    "PricePro as Price " +
                    " from dbo.Menu where " +
                    "(LOWER(NameProduct) LIKE N'%{0}%' OR LOWER(CodeProduct) LIKE N'%{0}%') AND " +
                    "( HasValue=1 and HasOnHand=1 and ok=1)",
                    q.Trim().ToLower());

            return  DataContext.Database.SqlQuery<MenuCategoryForAddPromotionmapping>(sql).ToList();
        }

        //lay danh sach san pham khi click vao danh muc theo link
        public IList<Menu> GetSanPhamTheoDanhMucByLink(string link)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and HasValue=1 and HasOnHand=1 AND MenuProAdd.idMenuCatelogy =(select M.id_ from Menu M where M.Link = '{0}')", link);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay danh sach danh muc level 2 theo menu top (level 1)
        public IList<Menu> GetDanhMucConCuaMenuTop(int id)
        {
            string sql = string.Format("select * from Menu where LevelMenu=2 and Menu.idControl={0}", id);
              return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay thuong hieu san pham
        public IList<Menu> GetThuongHieu(int id)
        {
            string sql = string.Format("select * from Menu where Menu.id_=(select IdNhaCungCap from Menu where id_={0})",id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //get id duong dan de them moi danh muc by link
        public int GetidDanhMucByLink(string link)
        {
            string sql = string.Format("select id_ from menu where Menu.Link='{0}'", link);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //lay ten danh muc de hien thi by ID
        public string GetTenDanhMucById(int id)
        {
            string sql = string.Format("select NameProduct from menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //lay ten danh muc cha by ID
        public string GetTenDanhMucChaById(int id)
        {
            string sql = string.Format("select NameProduct from menu where Menu.id_=(select idControl from menu where Menu.id_={0})", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }

        public IList<MenuCategoryTree>GetCategory()
        {
            return (from menu in DataContext.Menu
                    where menu.Style.Equals("danh-muc-san-pham") && menu.ok && menu.idControl != 0
                    select new MenuCategoryTree
                               {
                                   DanhMucId = menu.id_,
                                   Name = menu.NameProduct,
                                   ParentId = menu.idControl,
                                   Level = menu.LevelMenu,
                                   SapXepDanhMuc = menu.SapxepDanhMuc
                               }).OrderBy(o=>o.SapXepDanhMuc).ToList();

        }
        public int GetIdDanhMucCha(int id)
        {
            string sql = string.Format("select idControl from Menu where id_={0}",id);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }
        //lay danh sach san pham HOT thuoc danh muc ngoai man hinh
        public IList<Menu> GetSanPhamHotThuocDanhMuc(int id)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and HasValue=1 and HasOnHand=1 and Option1=1 AND MenuProAdd.idMenuCatelogy ={0}", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay danh sach san pham BAN CHAY thuoc danh muc ngoai man hinh
        public IList<Menu> GetSanPhamBanChayThuocDanhMuc(int id)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and HasValue=1 and HasOnHand=1 and Option6=1 AND MenuProAdd.idMenuCatelogy ={0}", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //kiem tra xem noi dung cua content co khong
        public  string CheckContent1(int id)
        {
            string sql = string.Format("select Content from Menu where Menu.id_={0}",id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //kiem tra xem noi dung cua content co khong
        public string CheckContent2(int id)
        {
            string sql = string.Format("select Content1 from Menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //kiem tra xem noi dung cua content co khong
        public string CheckContent3(int id)
        {
            string sql = string.Format("select Content2 from Menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //kiem tra xem noi dung cua content co khong
        public string CheckContent4(int id)
        {
            string sql = string.Format("select Content3 from Menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //kiem tra xem noi dung cua content co khong
        public string CheckContent5(int id)
        {
            string sql = string.Format("select Content4 from Menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        public bool CheckHasvalueTreOrFalse(int id)
        {
            string sql = string.Format("select HasValue from Menu where Menu.id_={0}", id);
            return this.DataContext.Database.SqlQuery<bool>(sql).FirstOrDefault();
        }
        //lay ten san pham dua len popup edit ma vach
        public string GetTenSanPham(int id)
        {
            string sql = string.Format("select NameProduct from Menu where id_={0}", id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        //lay danh sach san pham khi click tab san pham noi bat
        public IList<Menu> GetSanPhamNoiBatHome(int id)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and Option1=1 and HasValue=1 and HasOnHand=1 AND MenuProAdd.idMenuCatelogy ={0}", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay danh sach san pham khi click tab san pham ban chay
        public IList<Menu> GetSanPhamBanChayHome(int id)
        {
            string sql = string.Format("SELECT * from  Menu, MenuProAdd WHERE Menu.id_ = MenuProAdd.idMenuProAdded AND Menu.Style = 'san-pham' AND MenuProAdd.Style = 'add-san-pham' AND ok = 1 and Option6=1 and HasValue=1 and HasOnHand=1 AND MenuProAdd.idMenuCatelogy ={0}", id);
            return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
        }
        //lay danh sach san pham thuoc thuong hieu nao do
        public IList<Menu> getSanPhamThuocThuongHieu(string link)
        {
            string sql = string.Format("select (select Menu  from Menu NCC where NCC.id_ = Menu.idNhaCungCap) as NhaCungCap, * from Menu where Menu.IdNhaCungCap=(select id_ from Menu where Menu.Link='{0}') and HasValue=1", link);
             return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();

        }

        //lay id cua tin tuc footer tu link footer
        public int GetIdMenuFromLinkFooter(string linkfooter)
        {
            string sql = string.Format("select id_ from Menu where Link='{0}'", linkfooter);
            return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        }

        public IList<ProductFrontPageMapping.ProductBox> GetProductSeller()
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.HasOnHand && menu.ok
                  orderby menu.SoLanXem descending 

                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link
                       
                    })
                    .Take(6)
                    .ToList();
        }

        public IList<ProductFrontPageMapping.ProductBox> GetAllProductsHasPromotion(IList<int>ids )
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.HasOnHand && menu.ok && ids.Contains(menu.id_)
                    orderby menu.sDate descending
                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link,

                    })
                    .ToList();
        }

        public IList<ProductFrontPageMapping.ProductBox> GetProductNew(int page=5)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.HasOnHand && menu.ok &&menu.DungSai
                    && menu.idControl==11
                    orderby menu.sDate descending

                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link          
                    })
                    .Take(page)
                    .ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetProductNewInHome()
        {
            IList<Menu> menus = GetAllMenuCache();
            if (menus.Any())
                return
                    (from menu in
                         menus
                     where
                         menu.idControl == 11 &&
                         menu.HasValue &&
                         menu.ok && menu.DungSai
                     orderby menu.sDate descending
                     select
                         new ProductFrontPageMapping.
                         ProductBox()
                             {
                                 id_ = menu.id_,
                                 Img = menu.Img,
                                 NameProduct =
                                 menu.NameProduct,
                                 PricePro = menu.PricePro,
                                 Link = menu.Link,
                                 NgayHetHang = menu.NgayHetHang,
                                 HasOnHand = menu.HasOnHand
                             })
                        .Take(12)
                        .ToList();

            return new List<ProductFrontPageMapping.ProductBox>();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetProducGiaTotMoiNgay()
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.HasOnHand && menu.ok &&menu.GiaHot 
                    orderby menu.sDate descending

                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link

                    })
                    .Take(12)
                    .ToList();
        }
     
        public IList<ProductFrontPageMapping.ProductBox> GetProductNewCungDanhMuc(int iddanhmuc)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu1 in DataContext.Menu.AsNoTracking() on menuProAdd.idMenuProAdded equals menu1.id_

                    where menuProAdd.idMenuCatelogy == iddanhmuc &&menu1.HasValue&&menu1.HasOnHand&&menu1.ok &&menu1.DungSai
                    orderby menuProAdd.sDate descending
                    select new ProductFrontPageMapping.ProductBox()
                               {
                                   id_ = menu1.id_,
                                   Img = menu1.Img,
                                   NameProduct = menu1.NameProduct,
                                   PricePro = menu1.PricePro,
                                   Link = menu1.Link

                               }).Take(5).ToList();


        }

        public ProductFrontPageMapping.ProductForViewDetail GetForViewDetailByLink(string link)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.Link.Trim().ToLower().Equals(link.Trim().ToLower())
                    select new ProductFrontPageMapping.ProductForViewDetail
                    {
                        ProductId = menu.id_,
                        Link = menu.Link,
                        Note = menu.Note,
                        Img = menu.Img,
                        ContentLabel = menu.ContentLabel,
                        ContentLabel1 = menu.ContentLabel1,
                        Content = menu.Content,
                        Content1 = menu.Content1,
                        ContentLabel2 = menu.ContentLabel2,
                        Content2 = menu.Content2,
                        Option2 = menu.Option2,
                        ContentLabel3 = menu.ContentLabel3,
                        Content3 = menu.Content3,
                        ContentLabel4 = menu.ContentLabel4,
                        Content4 = menu.Content4,
                        BrandId = menu.IdNhaCungCap,
                        SEOKeyWord = menu.SEOKeyWord,
                        SEODescription = menu.SEODescription,
                        BarCode = menu.BarCode,
                        BarcodeType = menu.BarcodeType,
                        PricePro = menu.PricePro,
                        NameProduct = menu.NameProduct,
                        ContentLabelTaiSao = menu.ContentLabelTaiSao,
                        ContentTaiSao = menu.ContentTaiSao,
                        ContentTheoSp = menu.ContentTheoSp,
                        GiaThiTruong = menu.GiaThiTruong,
                        NameProductLong = menu.NameProductLong,
                        SEOtitle = menu.SEOtitle,
                        HasOnHand = menu.HasOnHand,
                        NgayHetHang = menu.NgayHetHang,
                        IdDanhMuc = (from menuProAdd in DataContext.MenuProAdd  where menuProAdd.idMenuProAdded==menu.id_ select menuProAdd.idMenuCatelogy).FirstOrDefault()
                    }).FirstOrDefault();

        }

        public ProductFrontPageMapping.ProductForViewDetail GetProductForTag(string link)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.Link.Trim().ToLower().Equals(link.Trim().ToLower()) 
                    select new ProductFrontPageMapping.ProductForViewDetail
                    {
                        ProductId = menu.id_,
                        Link = menu.Link,
                        Note = menu.Note,
                        Img = menu.Img,
                        ContentLabel = menu.ContentLabel,
                        ContentLabel1 = menu.ContentLabel1,
                        Content = menu.Content,
                        Content1 = menu.Content1,
                        ContentLabel2 = menu.ContentLabel2,
                        Content2 = menu.Content2,
                        Option2 = menu.Option2,
                        ContentLabel3 = menu.ContentLabel3,
                        Content3 = menu.Content3,
                        ContentLabel4 = menu.ContentLabel4,
                        Content4 = menu.Content4,


                        BrandId = menu.IdNhaCungCap,
                        SEOKeyWord = menu.SEOKeyWord,
                        SEODescription = menu.SEODescription,
                        BarCode = menu.BarCode,
                        BarcodeType = menu.BarcodeType,
                        PricePro = menu.PricePro,
                        NameProduct = menu.NameProduct,
                        ContentLabelTaiSao = menu.ContentLabelTaiSao,
                        ContentTaiSao = menu.ContentTaiSao,
                        ContentTheoSp = menu.ContentTheoSp,
                        GiaThiTruong = menu.GiaThiTruong,
                        NameProductLong = menu.NameProductLong,
                        IdDanhMuc = (from menuProAdd in DataContext.MenuProAdd where menuProAdd.idMenuProAdded == menu.id_ select menuProAdd.idMenuCatelogy).FirstOrDefault()
                    }).FirstOrDefault();

        }

        public ProductFrontPageMapping.Brand GetBrandById(int id)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.id_ == id
                    select new ProductFrontPageMapping.Brand
                    {
                        Name = menu.NameProduct
                    }).FirstOrDefault();
        }

       
        public IList<ProductFrontPageMapping.Attribute> GetAttributeByProductId(int productId)
        {
            return (from menuOption in DataContext.MenuOptions.AsNoTracking()
                    where menuOption.IdMenu == productId
                    select new ProductFrontPageMapping.Attribute
                    {
                        id_ = menuOption.id_,
                        Barcode = menuOption.Barcode,
                        Img = menuOption.Img,
                        Name = menuOption.TenLoai,
                        Flag = menuOption.Flag,
                        ProductId = menuOption.IdMenu,
                    }).ToList();
        }

        public IList<ProductFrontPageMapping.ProductShowCart> GetProductShowCartByBarcode(IList<string> barcodes)
        {
            return (from menuOption in DataContext.MenuOptions.AsNoTracking()
                    join menu in DataContext.Menu.AsNoTracking() on menuOption.IdMenu equals menu.id_
                    where barcodes.Contains(menuOption.Barcode)
                    select new ProductFrontPageMapping.ProductShowCart
                    {
                        ProductId = menu.id_,
                        Img = menu.Img,
                        PricePro = menu.PricePro,
                        NameProduct = menu.NameProduct,
                        Barcode = menuOption.Barcode,
                        Link = menu.Link,
                        AttributeFlag = menuOption.Flag,
                        AttributeImg = menuOption.Img,
                        AttributeName = menuOption.TenLoai
                    }).ToList();
        }


       public IList<ProductFrontPageMapping.ThuongHieuNoiBat> GetDSThuongHieuCount()
       {
           return (from menu in DataContext.Menu.AsNoTracking()
                   where menu.Style.Equals("thuong-hieu") && menu.idControl != 0 && menu.ok 
                 
                   select new ProductFrontPageMapping.ThuongHieuNoiBat
                              {
                                  id_ = menu.id_,
                                  Name = menu.NameProduct,
                                  Link = menu.Link,
                                  Quantity = (from menu1 in DataContext.Menu  
                                              where menu1.IdNhaCungCap==menu.id_ && menu1.HasOnHand && menu1.ok && menu1.HasValue

                                              select menu1.id_).Count()
                              })
                              
                              .OrderByDescending(o=>o.Quantity)
                              .Take(11).ToList();
       }
        public IList<ProductFrontPageMapping.ProductBox> GetSanPhamMuaCungHasProMotion(int idsanpham,int idnhacungcap)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.HasValue && menu.ok &&menu.IdNhaCungCap==idnhacungcap&&menu.id_!=idsanpham 
                    orderby menu.sDate descending
                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link,
                        HasOnHand = menu.HasOnHand,
                        NgayHetHang = menu.NgayHetHang
                     })
                  .Take(8)
                  .ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetSanPhamCungLoaiHasProMotion(int iddanhmuc)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu1 in DataContext.Menu.AsNoTracking() on menuProAdd.idMenuProAdded equals menu1.id_
                    where menuProAdd.idMenuCatelogy == iddanhmuc && menu1.HasValue && menu1.ok 
                    orderby menuProAdd.sDate descending
                    select new ProductFrontPageMapping.ProductBox()
                    {
                        id_ = menu1.id_,
                        Img = menu1.Img,
                        NameProduct = menu1.NameProduct,
                        PricePro = menu1.PricePro,
                        Link = menu1.Link,
                        HasOnHand = menu1.HasOnHand,
                        NgayHetHang = menu1.NgayHetHang

                    }).Take(10).ToList();
        }


        public IList<ProductFrontPageMapping.CategoryBoxHome> GetCategoryForBoxHome()
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.ok && menu.Style == "danh-muc-san-pham" && menu.idControl == 1 && menu.ShowMenuHome
                    select new ProductFrontPageMapping.CategoryBoxHome
                    {
                        Icon = menu.IconMenu,
                        Link = menu.Link,
                        Name = menu.NameProduct,
                        Id = menu.id_,

                    }).ToList();
        }

        public IList<ProductFrontPageMapping.CategoryBoxHome> GetCategoryChild(int id)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                    where menu.idControl == id && menu.LevelMenu == 2
                    select new ProductFrontPageMapping.CategoryBoxHome
                    {
                        Icon = menu.IconMenu,
                        Link = menu.Link,
                        Name = menu.NameProduct,
                        Id = menu.id_,
                    }).ToList();
        }

        public IList<ProductFrontPageMapping.ProductShow> GetProductsByCategory(int id)
        {
            return (from menuProAdd in DataContext.MenuProAdd
                    join menu2 in DataContext.Menu on menuProAdd.idMenuProAdded equals menu2.id_
                    where menuProAdd.idMenuCatelogy == id && menu2.ok && menu2.HasOnHand && menu2.HasValue
                    select new ProductFrontPageMapping.ProductShow
                    {
                        ProductId = menu2.id_,
                        Link = menu2.Link,
                        Img = menu2.Img,
                        PricePro = menu2.PricePro,
                        NameProduct = menu2.NameProduct,
                        sDate = menu2.sDate,
                        SoLanXem = menu2.SoLanXem,
                        Option1 = menu2.Option1
                    }).ToList();
        }

        public IList<int> LayTatCaIdConCuaDanhMuc(int id)
        {
            return (from menu in DataContext.Menu.AsNoTracking()
                where menu.idControl == id
                select menu.id_).ToList();
        }
        public IList<int>LatDanhSachIdCuaDanhMuc(int id)
        {
            string sql = string.Format("select p.id_ " +
                                       "from Menu as p " +
                                       "WHERE p.Style = 'danh-muc-san-pham' and p.idControl != 0 AND ( p.id_ = {0} OR (p.id_ IN (SELECT id_ FROM Menu WHERE  idControl={0})) " +
                                       "OR (p.id_ IN (SELECT id_ FROM Menu WHERE idControl IN (SELECT id_ FROM Menu WHERE idControl = {0}))))",id);

            return DataContext.Database.SqlQuery<int>(sql).ToList();
        } 

        public IList<ProductFrontPageMapping.ProductBox> LayTatCaSanPhamCuaListDanhMua(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_

                    where ids.Contains(menuProAdd.idMenuCatelogy) && menu.HasValue && menu.ok 
                   // group menu by menu.id_ into gr
                    
                    select new ProductFrontPageMapping.ProductBox
                    {
                        id_ = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link,
                        sDate = menu.sDate,
                        NameProductLong = menu.NameProduct,
                        HasOnHand = menu.HasOnHand,
                        NgayHetHang = menu.NgayHetHang
                    })
                    .Distinct()
                    .ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> LayTatCaSanPhamCuaDanhMucDeOrder(int iddanhmuc)
        {
            string sql = string.Format("SELECT * FROM  Menu LEFT JOIN MenuProAdd A ON A.idMenuProAdded = B.id_)"+
                "LEFT JOIN (SELECT COUNT(Id) AS CountRating ,IdSanPham FROM UserRatings GROUP BY IdSanPham"+
                "userrating ON B.id_ = userrating.IdSanPham WHERE (A.idMenuCatelogy = {0}"+
                    "OR idMenuCatelogy IN ( SELECT id_ FROM Menu WHERE idControl = {0})" +
                    "OR idMenuCatelogy IN ( SELECT id_ FROM Menu WHERE idControl IN ( SELECT id_ FROM Menu WHERE idControl = {0} )))"+
                    "AND B.NameProduct IS NOT NULL AND B.ok = 1 AND HasValue = 1 AND HasOnHand = 1"+
                    "ORDER BY userrating.CountRating DESC",iddanhmuc);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductBox>(sql).ToList();
        }

        public IList<ProductFrontPageMapping.ProductBox> LayTatCaSanphamTheoTag(string tentag)
        {
            string sql =
                string.Format("select M.* from Menu M inner join DanhSachTags tag on m.id_=tag.IdMenu where TenTag = N'{0}' and M.HasOnHand=1 and M.ok=1 and M.HasValue=1", tentag);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductBox>(sql).ToList();
        }

        public IList<ProductFrontPageMapping.ProductShow> LaySanPhamCuaListDanhMuc(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_

                    where ids.Contains(menuProAdd.idMenuCatelogy) && menu.HasValue && menu.HasOnHand && menu.ok &&menu.DungSai
                   
                    group menu by menu.id_ into gr

                    select new ProductFrontPageMapping.ProductShow
                    {
                        ProductId = gr.Key,
                        Img = gr.FirstOrDefault().Img,
                        NameProduct = gr.FirstOrDefault().NameProduct,
                        PricePro = gr.FirstOrDefault().PricePro,
                        Link = gr.FirstOrDefault().Link,
                        sDate = gr.FirstOrDefault().sDate,
                    }).ToList();
        }

        public IList<ProductFrontPageMapping.ProductShow> LaySanPhamCuaListDanhMucTrangChu(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_
                    where ids.Contains(menuProAdd.idMenuCatelogy) && menu.HasValue && menu.ok && menu.DungSai && menu.HasOnHand
                    orderby menu.id_ descending 
                   // group menu by menu.id_ into gr
                    select new ProductFrontPageMapping.ProductShow
                    {
                        ProductId = menu.id_,
                        Img = menu.Img,
                        NameProduct = menu.NameProduct,
                        PricePro = menu.PricePro,
                        Link = menu.Link,
                        sDate = menu.sDate,
                        HasOnHand = menu.HasOnHand,
                        NgayHetHang = menu.NgayHetHang
                    })
                    .Distinct()
                    .Take(12)
                    .ToList();
        }

        public int CountTatCaSanPhamCuaListDanhMucTheoDieuKien(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_

                where ids.Contains(menuProAdd.idMenuCatelogy) && menu.HasValue && menu.HasOnHand && menu.ok
                group menu by menu.id_
                into gr

                select gr.Key).Count();
        }
        public int CountTatCaSanPhamCuaListDanhMucKhongDieuKien(IList<int> ids)
        {
            return (from menuProAdd in DataContext.MenuProAdd.AsNoTracking()
                    join menu in DataContext.Menu on menuProAdd.idMenuProAdded equals menu.id_

                    where ids.Contains(menuProAdd.idMenuCatelogy) 
                    group menu by menu.id_
                into gr

                    select gr.Key).Count();
        }
        public string GetLydoNenMua()
        {
            string sql = string.Format("select ContentTaiSao from Menu where idControl=114");
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
        public IList<int> GetIdMenuByTenTag(string tentag)
        {
            string sql = string.Format("select M.id_ from Menu M inner join DanhSachTags tag on m.id_=tag.IdMenu where TenTag = '{0}' ", tentag);
            return this.DataContext.Database.SqlQuery<int>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> LaySanPhamYeuThichs(string listids)
        {
            string sql = string.Format("select * from Menu where id_ IN ({0})",listids);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductBox>(sql).ToList();
        }
       public IList<Menu> GetProductIds()
       {
           string sql = string.Format("select id_ from menu where idcontrol=11 and HasValue=1 and ok=1 and HasOnHand=1");
           return this.DataContext.Database.SqlQuery<Menu>(sql).ToList();
       }
        public IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv1()
        {
            string sql = string.Format(@"SELECT
	                                        A.idControl AS idControl
	                                        ,A.NameProduct AS CategoryName
	                                        ,A.LevelMenu AS LevelMenu
	                                        ,A.id_ AS id_
                                            ,A.Link
                                            ,A.IconMenu 
                                            ,A.ShowMenuHome
                                            ,A.SapxepDanhMuc
                                        FROM Menu A
                                        WHERE A.Style = 'danh-muc-san-pham' AND A.idControl != 0");
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.CategoryBoxHomeLv1>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv2(int iddanhmuc)
        {
            string sql = string.Format("select * from Menu where id_= {0}",iddanhmuc);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.CategoryBoxHomeLv1>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv3(int iddanhmuc)
        {
            string sql = string.Format(@"SELECT
	                                        A.idControl AS idControl
	                                        ,A.NameProduct AS CategoryName
	                                        ,A.LevelMenu AS LevelMenu
	                                        ,A.id_ AS Id
	                                        ,A.Link as Link
	                                        ,A.IconMenu 
	                                        ,A.ShowMenuHome
                                        FROM Menu A
                                        WHERE A.Style = 'danh-muc-san-pham' AND A.idControl != 0
	                                        AND A.idControl = {0} AND A.LevelMenu = 3",iddanhmuc);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.CategoryBoxHomeLv1>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetIdsFromIdanhmuc(int iddanhmuc)
        {
            string sql = string.Format("select id_ from Menu where idControl ={0} and LevelMenu=3", iddanhmuc);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.CategoryBoxHomeLv1>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.ProductShow> GetProducByIdDanhMuc(int iddanhmuc)
        {
            string sql = string.Format(@"SELECT TOP 12 
		                                    M.id_ as ProductId
		                                    ,M.Img as Img
		                                    ,M.NameProduct as NameProduct
		                                    ,M.PricePro as PricePro
		                                    ,M.Link as Link
	                                    FROM Menu M
	                                    WHERE id_ IN (SELECT idMenuProAdded 
				                                      FROM MenuProAdd 
				                                      WHERE idMenuCatelogy={0} 
						                                    OR idMenuCatelogy IN (
												                                    SELECT id_ 
												                                    FROM Menu 
												                                    WHERE idControl ={0} and LevelMenu=3 )
												                                    )
			                                    AND M.HasValue=1 
			                                    AND M.HasOnHand=1
			                                    AND M.ok=1
			                                    AND M.DungSai=1",iddanhmuc);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductShow>(sql).ToList();
        }
        public string GetLinkById(int id)
        {
            string sql = string.Format("select Link from menu where Menu.id_={0}",id);
            return this.DataContext.Database.SqlQuery<string>(sql).FirstOrDefault();
        }
         public int GetIdspByCommentId(int idcomment)
         {
             string sql = string.Format("select  IdSanPham from comments where CommentId={0}", idcomment);
             return this.DataContext.Database.SqlQuery<int>(sql).FirstOrDefault();
         }

        public Menu GetBreadcrumbDanhMuc(int idsanpham)
        {
            string sql = string.Format("select * from Menu where Menu.id_=(select top 1 idMenuCatelogy from MenuProAdd where idMenuProAdded={0})", idsanpham);
            return this.DataContext.Database.SqlQuery<Menu>(sql).FirstOrDefault();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetProductQuaTang()
        {
            return (from khoquatang in DataContext.KhoQuaTangs.AsNoTracking()
                join menu in DataContext.Menu.AsNoTracking() 
                on khoquatang.IdMenu equals menu.id_
                where menu.HasValue && menu.HasOnHand && menu.ok && menu.DungSai
                orderby khoquatang.NgayTao descending
                select new ProductFrontPageMapping.ProductBox()
                       {
                           id_ = menu.id_,
                           Img = menu.Img,
                           NameProduct = menu.NameProduct,
                           PricePro = menu.PricePro,
                           Link = menu.Link,
                           Note = menu.Note
                       }).ToList();
        }
        public IList<ProductFrontPageMapping.ProductBox> GetProductQuaTangByproductId(int productId)
        {
            //return (from menu in DataContext.Menu.AsNoTracking()
            //    join khoquatang in DataContext.KhoQuaTangs.AsNoTracking()
            //    on menu.id_ equals khoquatang.IdMenu
            //    where menu.HasValue && menu.HasOnHand && menu.ok && menu.DungSai && menu.id_ == productId
            //    select new ProductFrontPageMapping.ProductBox()
            //           {
            //               id_ = menu.id_,
            //               Img = menu.Img,
            //               NameProduct = menu.NameProduct,
            //               PricePro = menu.PricePro,
            //               Link = menu.Link
            //           }).ToList();

            string sql = string.Format(@"select top 1
	                                    A.id_
	                                    ,A.Img
	                                    ,A.NameProduct
	                                    ,A.PricePro
	                                    ,A.Link
	                                     from Menu A
	                                    left join KhoQuaTangs B on  A.id_ = B.IdMenu
	                                    where A.id_ = {0}",productId);
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductBox>(sql).ToList();
        }
        public IList<ProductFrontPageMapping.ProductShow> GetProductBestSeller()
        {
            string sql = string.Format(@";WITH s AS 
	                                    (
		                                      SELECT
			                                    Distinct muanhieu.Link
			                                    ,ROW_NUMBER() OVER (ORDER BY COUNT(muanhieu.id_) DESC) AS 'RowNum'	
			                                    ,COUNT(muanhieu.id_) AS solanmua
		                                    FROM DetailMenuCommentItem muanhieu
		                                    LEFT JOIN Menu B ON B.id_ = muanhieu.Link
											WHERE B.ok=1 and B.HasValue=1 and HasOnHand = 1
		                                    GROUP BY muanhieu.Link
		                                    HAVING COUNT(muanhieu.id_) IS NOT NULL
	                                    )
	                                    SELECT 
		
		                                     B.id_, B.Img,B.NameProduct,B.PricePro,B.Link,B.sdate,B.HasOnHand,B.NgayHetHang
	                                    FROM s
	                                    LEFT JOIN Menu B ON B.id_ = s.Link
	                                    WHERE s.RowNum <= 12 ");
            return this.DataContext.Database.SqlQuery<ProductFrontPageMapping.ProductShow>(sql).ToList();
        }

        public IList<ProductFrontPageMapping.ProductBox> GetProductBestsellerShowHome()
        {
            IList<Menu> menus = GetAllMenuCache();
            if (menus.Any())
                return
                    (from menu in
                            menus
                        where
                            menu.idControl == 11 &&
                            menu.HasValue &&
                            menu.ok && menu.DungSai &&
                            menu.HasOnHand &&
                            menu.Bestseller
                        orderby menu.sDate descending
                        select
                            new ProductFrontPageMapping.
                                ProductBox()
                                {
                                    id_ = menu.id_,
                                    Img = menu.Img,
                                    NameProduct =
                                        menu.NameProduct,
                                    PricePro = menu.PricePro,
                                    Link = menu.Link,
                                    NgayHetHang = menu.NgayHetHang,
                                    HasOnHand = menu.HasOnHand,
                                    Bestseller = menu.Bestseller
                                })
                    .Take(12)
                    .ToList();

            return new List<ProductFrontPageMapping.ProductBox>();
        }
    }
    public interface IMenuRepository : IRepository<Menu>
    {
        IList<Menu> TimKiem(string key);
        int ChekBarcode(string mavach);
        int CheckOnhand(string mavach);
        int GetIdMenu();
        IList<Menu> TimKiemByTenAndBarcode(string key);
        IList<Menu> UpdateHasvalue(int id, bool hasvalue);
        IList<Menu> UpdateHasOnhand(int id, bool hasOnhand);
        string GetTenDanhMuc(string link);
        string GetTenDanhMucCha(string link);
        string GetLinkDanhMucCha(string link);
        int GetidDanhMuc(int id);
        IList<Menu> GetSanPhamCungLoai(int id);
        IList<Menu> GetSanPhamMuaCung(int id);
        IList<Menu> GetSanPhamTheoDanhMuc(int id);
        IList<Menu> GetSanPhamNoiBatHome(int id);
        IList<Menu> GetSanPhamBanChayHome(int id);
        IList<Menu> GetSanPhamTheoDanhMucByLink(string link);
        IList<Menu> GetDanhMucConCuaMenuTop(int id);
        IList<Menu> GetThuongHieu(int id);
        int GetidDanhMucByLink(string link);
        string GetTenDanhMucById(int id);
        string GetTenDanhMucChaById(int id);
        int GetIdDanhMucCha(int id);
        IList<Menu> GetSanPhamHotThuocDanhMuc(int id);
        IList<Menu> GetSanPhamBanChayThuocDanhMuc(int id);
        IList<MenuCategoryTree> GetCategory();
        string CheckContent1(int id);
        string CheckContent2(int id);
        string CheckContent3(int id);
        string CheckContent4(int id);
        string CheckContent5(int id);
        bool CheckHasvalueTreOrFalse(int id);
        string GetTenSanPham(int id);
        IList<Menu> getSanPhamThuocThuongHieu(string link);
        int GetIdMenuFromLinkFooter(string linkfooter);
        string GetLydoNenMua();


        IList<MenuCategoryForAddPromotionmapping> GetListProductsByCategoryIds(IList<int> ids);
        IList<MenuCategoryForAddPromotionmapping> GetByIds(IList<int> ids);
        IList<MenuCategoryForAddPromotionmapping> GetAllProductsForPromotion();
        IList<MenuCategoryForAddPromotionmapping> AutocompleteSearch(string q);


        ProductFrontPageMapping.ProductForViewDetail GetForViewDetailByLink(string link);
        ProductFrontPageMapping.ProductForViewDetail GetProductForTag(string link);
       
        ProductFrontPageMapping.Brand GetBrandById(int id);
        IList<ProductFrontPageMapping.Attribute> GetAttributeByProductId(int productId);
        IList<ProductFrontPageMapping.ProductShowCart> GetProductShowCartByBarcode(IList<string> barcodes);




        IList<ProductFrontPageMapping.ProductBox> GetProductSeller();
        IList<ProductFrontPageMapping.ProductBox> GetProductNew(int page);
        IList<ProductFrontPageMapping.ProductBox> GetProducGiaTotMoiNgay();
        IList<ProductFrontPageMapping.ProductBox> GetProductNewInHome();
        IList<ProductFrontPageMapping.ProductBox> GetProductNewCungDanhMuc(int iddanhmuc);
        IList<ProductFrontPageMapping.ThuongHieuNoiBat> GetDSThuongHieuCount();
        IList<ProductFrontPageMapping.ProductBox> GetSanPhamMuaCungHasProMotion(int idsanpham, int idnhacungcap);
        IList<ProductFrontPageMapping.ProductBox> GetSanPhamCungLoaiHasProMotion(int iddanhmuc);


        IList<ProductFrontPageMapping.CategoryBoxHome> GetCategoryForBoxHome();
        IList<ProductFrontPageMapping.CategoryBoxHome> GetCategoryChild(int id);
        IList<ProductFrontPageMapping.ProductShow> GetProductsByCategory(int id);

        IList<int> LayTatCaIdConCuaDanhMuc(int id);
        IList<ProductFrontPageMapping.ProductBox> LayTatCaSanPhamCuaListDanhMua(IList<int> ids);

        IList<ProductFrontPageMapping.ProductBox> LayTatCaSanPhamCuaDanhMucDeOrder(int iddanhmuc);

        IList<ProductFrontPageMapping.ProductShow> LaySanPhamCuaListDanhMuc(IList<int> ids);

        int CountTatCaSanPhamCuaListDanhMucTheoDieuKien(IList<int> ids);
        int CountTatCaSanPhamCuaListDanhMucKhongDieuKien(IList<int> ids);

        IList<int> LatDanhSachIdCuaDanhMuc(int id);
        IList<ProductFrontPageMapping.ProductBox> GetAllProductsHasPromotion(IList<int> ids);
        IList<int> GetIdMenuByTenTag(string tentag);
        IList<ProductFrontPageMapping.ProductBox> LayTatCaSanphamTheoTag(string tentag);
        IList<ProductFrontPageMapping.ProductBox> LaySanPhamYeuThichs(string listids);
        IList<Menu> GetProductIds();
        IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv1();
        IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv2(int iddanhmuc);
        IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetCatagoryLv3(int iddanhmuc);
        IList<ProductFrontPageMapping.CategoryBoxHomeLv1> GetIdsFromIdanhmuc(int iddanhmuc);
        IList<ProductFrontPageMapping.ProductShow> GetProducByIdDanhMuc(int iddanhmuc);
        string GetLinkById(int id);

        IList<Menu> GetAllMenuCache();
        
        //banner
        IList<ProductFrontPageMapping.BannerShowHome> GetBannersHome();
        IList<ProductFrontPageMapping.BrandMapping> GetAllBrandCache();
        void ClearCacheByKey(IList<string> keys);
        void ClearCacheBannerHome(string key);
        IList<ProductFrontPageMapping.ProductShow> LaySanPhamCuaListDanhMucTrangChu(IList<int> ids);
        int GetIdspByCommentId(int idcomment);
        Menu GetBreadcrumbDanhMuc(int idsanpham);
        IList<ProductFrontPageMapping.ProductShow> GetProductBestSeller();
        IList<ProductFrontPageMapping.ProductBox> GetProductQuaTang();
        IList<ProductFrontPageMapping.ProductBox> GetProductQuaTangByproductId(int productId);
        IList<ProductFrontPageMapping.ProductBox> GetProductBestsellerShowHome();
    }
}

 