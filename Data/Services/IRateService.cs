using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IRateService
    {
        List<Rate> GetAll();
        IQueryable<Rate> GetRateByProductId(long id);
        List<int> GetRateStarByUserId(long userId, long productId);
        Rate Insert(Rate rate);
    }
}
