using Common;
using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class NewsCategoryService : INewsCategoryService
    {
        TGClothesDbContext db = null;
        public NewsCategoryService()
        {
            db = new TGClothesDbContext();
        }

        public bool ChangeStatus(long id)
        {
            var category = db.Categories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status;
        }

        public bool Delete(long id)
        {
            try
            {
                var data = db.Categories.Find(id);
                db.Categories.Remove(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Category> GetAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }

        public Category GetById(long id)
        {
            return db.Categories.Find(id);
        }

        public long Insert(Category category)
        {
            if (string.IsNullOrEmpty(category.MetaTitle))
            {
                category.MetaTitle = StringHelper.ToUnsignString(category.Name);
            }
            category.CreatedDate = DateTime.Now;
            category.Status = true;
            db.Categories.Add(category);
            db.SaveChanges();
            return category.Id;
        }

        public bool Update(Category category)
        {
            try
            {
                var data = db.Categories.Find(category.Id);
                data.Name = category.Name;
                data.MetaTitle = category.MetaTitle;
                data.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
