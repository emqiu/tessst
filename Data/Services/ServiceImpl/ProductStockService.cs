using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class ProductStockService : IProductStockService
    {
        TGClothesDbContext db = null;
        public ProductStockService()
        {
            db = new TGClothesDbContext();
        }

        public List<ProductSize> GetAll()
        {
            return db.ProductSizes.ToList();
        }

        public List<ProductSize> GetProductSizeByProductId(long productId, long sizeId)
        {
            var product = db.Products.Find(productId);
            return db.ProductSizes.Where(x => x.ProductId == product.Id && x.SizeId == sizeId).ToList();
        }

        public List<ProductSize> GetProductSizeByProductId(long id)
        {
            return db.ProductSizes.Where(x => x.ProductId == id).ToList();
        }

        public ProductSize GetProductSizeByProductIdAndSizeId(long productId, long sizeId)
        {
            return db.ProductSizes.Where(x => x.ProductId == productId && x.SizeId == sizeId).FirstOrDefault();
        }

        public int GetStock(long productId, long sizeId)
        {
            return db.ProductSizes.Where(x => x.ProductId == productId && x.SizeId == sizeId).FirstOrDefault().Stock;
        }

        public void Insert(ProductSize productSize)
        {
            db.ProductSizes.Add(productSize);
            db.SaveChanges();
        }

        public void InsertMany(List<ProductSize> productSizes)
        {
            db.ProductSizes.AddRange(productSizes);
            db.SaveChanges();
        }

        public void Update(ProductSize productSize)
        {
            try
            {
                var data = db.ProductSizes.Find(productSize.ProductId, productSize.SizeId);
                data.Stock = productSize.Stock;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating ProductSize. Please try again later.", ex);
            }
        }
    }
}
