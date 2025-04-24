using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class RateService : IRateService
    {
        TGClothesDbContext db = null;
        public RateService()
        {
            db = new TGClothesDbContext();
        }

        public List<Rate> GetAll()
        {
            return db.Rates.ToList();
        }

        public IQueryable<Rate> GetRateByProductId(long id)
        {
            return db.Rates.Where(x => x.Star > 0 && x.ProductId == id);
        }

        public List<int> GetRateStarByUserId(long userId, long productId)
        {
            return db.Rates.Where(x => x.UserId == userId && x.ProductId == productId).Select(x => x.Star).ToList();
        }

        public Rate Insert(Rate rate)
        {
            db.Rates.Add(rate);
            db.SaveChanges();
            return rate;
        }
    }
}
