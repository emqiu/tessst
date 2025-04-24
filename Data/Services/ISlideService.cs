using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ISlideService
    {
        List<Slide> GetAll();
        Slide GetSlideById(long id);
        long Insert(Slide slide);
        bool Update(Slide slide);
        bool Delete(long id);
        bool ChangeStatus(long id);
    }
}
