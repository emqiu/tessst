using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface INewsCategoryService
    {
        List<Category> GetAll();
        long Insert(Category category);
        Category GetById(long id);
        bool Update(Category size);
        bool Delete(long id);
        bool ChangeStatus(long id);
    }
}
