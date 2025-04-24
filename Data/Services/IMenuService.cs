using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IMenuService
    {
        List<Menu> GetByGroupId(int groupId);
    }
}
