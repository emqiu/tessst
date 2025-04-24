using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface INewsService
    {
        long Create(Content news);
        long Edit(Content news);
        bool Delete(long id);
        void InsertTag(string id, string name);
        Content GetById(long id);
        IEnumerable<Content> GetAll(ref int totalRecord, int page, int pageSize);
        List<Content> GetAll();
        IEnumerable<Content> GetAllByTag(string tag, int page, int pageSize);
        IEnumerable<Content> GetAllByCategory(long categoryId, int page, int pageSize);
        IEnumerable<Content> GetAllPaging(string searchString, int page, int pageSize);
        List<Tag> ListTag(long id);
        Tag GetTag(string id);
        Category GetCategory(long id);
        List<Tag> GetAllTag();
        bool ChangeStatus(long id);
    }
}
