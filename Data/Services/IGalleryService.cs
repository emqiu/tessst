using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IGalleryService
    {
        long Insert(Gallery gallery);
        void Update(Gallery gallery);
        long GetLastGallery();
        Gallery GetGalleryById(long id);
        List<Gallery> GetAll();
    }
}
