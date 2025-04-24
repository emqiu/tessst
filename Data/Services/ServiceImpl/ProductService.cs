using Common;
using Data.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class ProductService : IProductService
    {
        TGClothesDbContext db = null;
        public ProductService()
        {
            db = new TGClothesDbContext();
        }

        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }

        public bool Delete(long id)
        {
            try
            {
                var productExist = (from p in db.Products
                               join od in db.OrderDetails on p.Id equals od.ProductId
                               where p.Id == id
                               select p).Any();
                if (productExist)
                {
                    return false;
                }
                else
                {
                    var product = db.Products.Find(id);
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Product> FilterByPrice(decimal minPrice, decimal maxPrice, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            totalRecord = db.Products.Where(x => x.Price >= minPrice && x.Price <= maxPrice).Count();
            return db.Products.Where(x => x.Price >= minPrice && x.Price <= maxPrice).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> GetAll(ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            var data = (from p in db.Products
                        join ps in db.ProductSizes on p.Id equals ps.ProductId
                        where ps.Stock != 0
                        select p).Distinct();
            totalRecord = data.Count();
            return data.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Product> GetAll()
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IEnumerable<Product> GetAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<Product> GetAllProductByRootCategory(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            var category = db.ProductCategories.Find(id);
            
            List<Product> products = new List<Product>();
            List<ProductCategory> productCategories = db.ProductCategories.Where(x => x.ParentId == category.Id).ToList();

            foreach (var item in productCategories)
            {
                var result = (from p in db.Products
                            join ps in db.ProductSizes on p.Id equals ps.ProductId
                            where p.CategoryId == item.Id && p.Status == true
                            select p).Distinct();
                products.AddRange(result);
            }
            totalRecord = products.Count();
            return products.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Product> GetProductByCategoryId(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            var data = (from p in db.Products
                        join ps in db.ProductSizes on p.Id equals ps.ProductId
                        where p.Status == true
                        select p).Distinct();
            totalRecord = data.Where(x => x.CategoryId == id).Count();
            return data.Where(x => x.CategoryId == id).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public Product GetProductById(long id)
        {
            return db.Products.Find(id);
        }

        public long Insert(Product product)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
            }
            db.Products.Add(product);
            db.SaveChanges();
            return product.Id;
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword) && x.Status == true).Select(x => x.Name).ToList();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRelateProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.Id != productId && x.CategoryId == product.CategoryId && x.Status == true).ToList();
        }

        public List<Product> ListSaleProduct(int top)
        {
            return db.Products.Where(x => x.Promotion.HasValue).OrderByDescending(x => x.Promotion.Value).Take(top).ToList();
        }

        public List<Product> ListSaleProducts()
        {
            return db.Products.Where(x => x.Promotion.HasValue).OrderByDescending(x => x.Promotion.Value).ToList();
        }

        public List<Product> ListTopProduct(int top)
        {
            var query = (from p in db.Products
                         join od in db.OrderDetails on p.Id equals od.ProductId
                         join o in db.Orders on od.OrderId equals o.Id
                         where o.DeliveryDate.HasValue && o.DeliveryDate.Value.Month == DateTime.Now.Month && o.Status == 3
                         group new { p, od } by p.Id into g
                         orderby g.Sum(x => x.od.Quantity) descending
                         select new
                         {
                             Id = g.Key,
                             Name = g.Select(x => x.p.Name).FirstOrDefault(),
                             Price = g.Select(x => x.p.Price).FirstOrDefault(),
                             MetaTitle = g.Select(x => x.p.MetaTitle).FirstOrDefault(),
                             Description = g.Select(x => x.p.Description).FirstOrDefault(),
                             Image = g.Select(x => x.p.Image).FirstOrDefault(),
                             PromotionPrice = g.Select(x => x.p.PromotionPrice).FirstOrDefault(),
                             Promotion = g.Select(x => x.p.Promotion).FirstOrDefault(),
                             CreatedDate = g.Select(x => x.p.CreatedDate).FirstOrDefault(),
                         }).Take(10);

            var data = query.ToList().Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Promotion = x.Promotion,
                Image = x.Image,
                PromotionPrice = x.PromotionPrice,
                Description = x.Description,
                MetaTitle = x.MetaTitle,
                CreatedDate = x.CreatedDate
            }).ToList();

            return data;
        }

        public double ProductStatistic()
        {
            return db.Products.Where(x => x.Status == true).Count();
        }

        public List<Product> Search(string searchkeyword, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(searchkeyword)).Count();
            return db.Products.Where(x => x.Name.Contains(searchkeyword)).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public bool Update(Product product)
        {
            var data = db.Products.Find(product.Id);
            if (data != null)
            {
                data.Name = product.Name;
                data.MetaTitle = StringHelper.ToUnsignString(product.Name); ;
                data.Description = product.Description;
                data.Image = product.Image;
                data.OriginalPrice = product.OriginalPrice;
                data.Price = product.Price;
                data.Promotion = product.Promotion;
                data.PromotionPrice = product.PromotionPrice;
                data.CategoryId = product.CategoryId;
                data.GalleryId = product.GalleryId;

                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
