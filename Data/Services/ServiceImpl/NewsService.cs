using Common;
using Data.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class NewsService : INewsService
    {
        TGClothesDbContext db = null;
        public NewsService()
        {
            db = new TGClothesDbContext();
        }

        public Content GetById(long id)
        {
            return db.Contents.Find(id);
        }

        public long Create(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.Status = true;
            db.Contents.Add(content);
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to News tag
                    this.InsertNewsTag(content.Id, tagId);

                }
            }

            return content.Id;
        }

        public long Edit(Content content)
        {
            var data = db.Contents.Find(content.Id);
            //Xử lý alias
            if (string.IsNullOrEmpty(data.MetaTitle))
            {
                data.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            data.Name = content.Name;
            data.Detail = content.Detail;
            data.Description = content.Description;
            data.CategoryId = content.CategoryId;
            data.Image = content.Image;
            data.Tags = content.Tags;
            data.ModifiedDate = DateTime.Now;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(data.Tags))
            {
                this.DeleteAllNewsTag(data.Id);
                string[] tags = data.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to News tag
                    this.InsertNewsTag(data.Id, tagId);

                }
            }

            return data.Id;
        }

        public void DeleteAllNewsTag(long NewsId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentId == NewsId));
            db.SaveChanges();
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.Id = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertNewsTag(long NewsId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentId = NewsId;
            contentTag.TagId = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.Id == id) > 0;
        }

        public List<Content> GetAll()
        {
            return db.Contents.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IEnumerable<Content> GetAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Content> GetAll(ref int totalRecord, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            totalRecord = db.Contents.Count();
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<Tag> ListTag(long id)
        {
            var model = (from t in db.Tags
                         join ct in db.ContentTags on t.Id equals ct.TagId
                         where ct.ContentId == id
                         select new
                         {
                             Id = ct.TagId,
                             Name = t.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             Id = x.Id,
                             Name = x.Name
                         });
            return model.ToList();
        }

        public IEnumerable<Content> GetAllByTag(string tag, int page, int pageSize)
        {
            var model = (from c in db.Contents
                         join ct in db.ContentTags on c.Id equals ct.ContentId
                         where ct.TagId == tag
                         select new
                         {
                             Id = c.Id,
                             Name = c.Name,
                             MetaTitle = c.MetaTitle,
                             Image = c.Image,
                             Description = c.Description,
                             CreatedDate = DateTime.Now
                         }).AsEnumerable().Select(x => new Content()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate
                         });
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Content> GetAllByCategory(long categoryId, int page, int pageSize)
        {
            var model = (from c in db.Contents
                         join cat in db.Categories on c.CategoryId equals cat.Id
                         where cat.Id == categoryId && c.Status == true
                         select new
                         {
                             Id = c.Id,
                             Name = c.Name,
                             MetaTitle = c.MetaTitle,
                             Image = c.Image,
                             Description = c.Description,
                             CreatedDate = DateTime.Now
                         }).AsEnumerable().Select(x => new Content()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate
                         });
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }

        public Category GetCategory(long id)
        {
            return db.Categories.Find(id);
        }

        public List<Tag> GetAllTag()
        {
            return db.Tags.ToList();
        }

        public bool Delete(long id)
        {
            try
            {
                var News = db.Contents.Find(id);
                db.Contents.Remove(News);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var News = db.Contents.Find(id);
            News.Status = !News.Status;
            db.SaveChanges();
            return News.Status;
        }
    }
}
