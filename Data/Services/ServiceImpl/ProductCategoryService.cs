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
    public class ProductCategoryService : IProductCategoryService
    {
        TGClothesDbContext db = null;
        public ProductCategoryService()
        {
            db = new TGClothesDbContext();
        }

        public long Insert(ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(productCategory.MetaTitle))
            {
                productCategory.MetaTitle = StringHelper.ToUnsignString(productCategory.Name);
            }
            productCategory.CreatedDate = DateTime.Now;
            db.ProductCategories.Add(productCategory);
            db.SaveChanges();
            return productCategory.Id;
        }

        public ProductCategory GetProductCategoryByName(string userName)
        {
            return db.ProductCategories.SingleOrDefault(x => x.Name == userName);
        }

        public IEnumerable<ProductCategory> GetAllPaging(int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            return model.OrderBy(x => x.DisplayOrder).ToPagedList(page, pageSize);
        }

        public bool Update(ProductCategory productCategory)
        {
            try
            {
                var data = db.ProductCategories.Find(productCategory.Id);
                data.Name = productCategory.Name;
                data.MetaTitle = productCategory.MetaTitle;
                data.DisplayOrder = productCategory.DisplayOrder;
                data.ParentId = productCategory.ParentId;
                data.Status = productCategory.Status;
                data.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ProductCategory GetProductCategoryById(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool Delete(long id)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var productCategory = db.ProductCategories.Find(id);
            productCategory.Status = !productCategory.Status;
            db.SaveChanges();
            return productCategory.Status;
        }

        public List<ProductCategory> GetAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).ToList();
        }

        public List<ProductCategory> GetCategoriesWithParentName()
        {
            List<ProductCategory> categories = db.ProductCategories.ToList();

            foreach (var category in categories)
            {
                if (category.ParentId != 0)
                {
                    // Tìm danh mục cha trong danh sách các danh mục
                    var parentCategory = categories.FirstOrDefault(c => c.Id == category.ParentId);

                    if (parentCategory != null)
                    {
                        // Gán tên của danh mục cha vào thuộc tính ParentName
                        category.Name = parentCategory.Name;
                    }
                }

                categories.Add(category);
            }

            return categories;
        }
    }
}
