using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class SlideService : ISlideService
    {
        TGClothesDbContext db = null;
        public SlideService()
        {
            db = new TGClothesDbContext();
        }

        public List<Slide> GetAll()
        {
            return db.Slides.Where(x => x.Status == true).ToList();
        }

        public Slide GetSlideById(long id)
        {
            return db.Slides.Find(id);
        }

        public long Insert(Slide slide)
        {
            slide.CreatedDate = DateTime.Now;
            slide.Status = true;
            db.Slides.Add(slide);
            db.SaveChanges();
            return slide.Id;
        }

        public bool Update(Slide slide)
        {
            try
            {
                var data = db.Slides.Find(slide.Id);
                data.Image = slide.Image;
                data.DisplayOrder = slide.DisplayOrder;
                data.ModifiedDate = DateTime.Now;
                data.Link = slide.Link;
                data.Description = slide.Description;
                data.Status = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                var slide = db.Slides.Find(id);
                db.Slides.Remove(slide);
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
            var slide = db.Slides.Find(id);
            slide.Status = !slide.Status;
            db.SaveChanges();
            return slide.Status;
        }
    }
}
