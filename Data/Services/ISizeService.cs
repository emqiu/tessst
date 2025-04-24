using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ISizeService
    {
        List<Size> GetAll();
        long Insert(Size size);
        Size GetSizeById(long id);
        bool Update(Size size);
        bool Delete(long id);
    }
}
