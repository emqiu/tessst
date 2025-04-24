using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IProductStockService
    {
        List<ProductSize> GetAll();
        List<ProductSize> GetProductSizeByProductId(long productId);
        ProductSize GetProductSizeByProductIdAndSizeId(long productId, long sizeId);
        int GetStock(long productId, long sizeId);
        void Insert(ProductSize productSize);
        void Update(ProductSize productSize);
        void InsertMany(List<ProductSize> productSizes);
    }
}
