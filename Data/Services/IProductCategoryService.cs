using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IProductCategoryService
    {
        long Insert(ProductCategory productCategory);
        ProductCategory GetProductCategoryByName(string userName);
        List<ProductCategory> GetCategoriesWithParentName();
        List<ProductCategory> GetAll();
        IEnumerable<ProductCategory> GetAllPaging(int page, int pageSize);
        bool Update(ProductCategory productCategory);
        ProductCategory GetProductCategoryById(long id);
        bool Delete(long id);
        bool ChangeStatus(long id);
    }
}
