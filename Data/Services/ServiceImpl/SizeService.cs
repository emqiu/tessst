using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class SizeService : ISizeService
    {
        TGClothesDbContext db = null;
        public SizeService()
        {
            db = new TGClothesDbContext();
        }

        public bool Delete(long id)
        {
            try
            {
                var size = db.Sizes.Find(id);
                db.Sizes.Remove(size);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Size> GetAll()
        {
            return db.Sizes.ToList();
        }

        public Size GetSizeById(long id)
        {
            return db.Sizes.Find(id);
        }

        public long Insert(Size size)
        {
            db.Sizes.Add(size);
            db.SaveChanges();
            return size.Id;
        }

        public bool Update(Size size)
        {
            try
            {
                var data = db.Sizes.Find(size.Id);
                data.Name = size.Name;
                data.Description = size.Description;
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
