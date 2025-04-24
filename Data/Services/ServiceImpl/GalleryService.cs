using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class GalleryService : IGalleryService
    {
        TGClothesDbContext db = null;
        public GalleryService()
        {
            db = new TGClothesDbContext();
        }

        public List<Gallery> GetAll()
        {
            return db.Galleries.ToList();
        }

        public Gallery GetGalleryById(long id)
        {
            return db.Galleries.Find(id);
        }

        public long GetLastGallery()
        {
            return db.Galleries.Last().Id;
        }

        public long Insert(Gallery gallery)
        {
            db.Galleries.Add(gallery);
            db.SaveChanges();
            return gallery.Id;
        }

        public void Update(Gallery gallery)
        {
            try
            {
                var data = db.Galleries.Find(gallery.Id);
                data.Image1 = gallery.Image1;
                data.Image2 = gallery.Image2;
                data.Image3 = gallery.Image3;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
