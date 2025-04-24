using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IProductService
    {
        long Insert(Product product);
        bool Update(Product product);
        bool Delete(long id);
        List<Product> GetAll();
        List<Product> GetAll(ref int totalRecord, int pageIndex = 1, int pageSize = 8);
        List<Product> GetAllProductByRootCategory(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 8);
        List<Product> Search(string searchkeyword, ref int totalRecord, int pageIndex = 1, int pageSize = 8);
        List<Product> FilterByPrice(decimal minPrice, decimal maxPrice, ref int totalRecord, int pageIndex = 1, int pageSize = 8);
        IEnumerable<Product> GetAllPaging(string searchString, int page, int pageSize);
        Product GetProductById(long id);
        List<Product> GetProductByCategoryId(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 8);
        List<string> ListName(string keyword);
        List<Product> ListNewProduct(int top);
        //List<Product> ListFeatureProduct(int top);
        List<Product> ListRelateProduct(long productId);
        List<Product> ListSaleProduct(int top);
        List<Product> ListSaleProducts();
        List<Product> ListTopProduct(int top);
        bool ChangeStatus(long id);
        double ProductStatistic();
    }
}
