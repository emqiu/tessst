using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class FeedbackService : IFeedbackService
    {
        TGClothesDbContext db = null;
        public FeedbackService()
        {
            db = new TGClothesDbContext();
        }

        public bool Delete(long id)
        {
            var data = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(data);
            db.SaveChanges();
            return true;
        }

        public List<Feedback> GetAll()
        {
            return db.Feedbacks.ToList();
        }

        public long InsertFeedback(Feedback feedback)
        {
            db.Feedbacks.Add(feedback);
            db.SaveChanges();
            return feedback.Id;
        }
    }
}
