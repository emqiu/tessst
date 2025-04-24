using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class MenuService : IMenuService
    {
        TGClothesDbContext db = null;
        public MenuService()
        {
            db = new TGClothesDbContext();
        }

        public List<Menu> GetByGroupId(int groupId)
        {
            return db.Menus.OrderBy(o => o.DisplayOrder).ToList();
        }
    }
}
