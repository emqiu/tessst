using Data.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class AccountService : IAccountService
    {
        TGClothesDbContext db = null;
        public AccountService()
        {
            db = new TGClothesDbContext();
        }

        public long Insert(Account user)
        {
            user.GroupId = Common.CommonConstants.MEMBER_GROUP;
            db.Accounts.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public int Login(string email, string password)
        {
            var result = db.Accounts.SingleOrDefault(x => x.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == password)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

        public IEnumerable<Account> GetAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Account> model = db.Accounts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Update(Account user)
        {
            try
            {
                var data = db.Accounts.Find(user.Id);
                data.Name = user.Name;
                data.Email = user.Email;
                data.Password = user.Password;
                data.ModifiedDate = DateTime.Now;
                data.ResetPasswordCode = user.ResetPasswordCode;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Account GetUserById(long id)
        {
            return db.Accounts.Find(id);
        }

        public bool Delete(long id)
        {
            try
            {
                var user = db.Accounts.Find(id);
                db.Accounts.Remove(user);
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
            var Account = db.Accounts.Find(id);
            Account.Status = !Account.Status;
            db.SaveChanges();
            return Account.Status;
        }

        public List<Account> GetAll()
        {
            return db.Accounts.ToList();
        }

        public bool CheckEmailExist(string email)
        {
            return db.Accounts.Count(x => x.Email == email) > 0;
        }

        public Account GetUserByEmail(string email)
        {
            return db.Accounts.SingleOrDefault(x => x.Email == email);
        }

        public int LoginByEmail(string email, string password, bool isLoginAdmin = false)
        {
            var result = db.Accounts.SingleOrDefault(x => x.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupId == Common.CommonConstants.ADMIN_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == password)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == password)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }

        public long InsertForFacebook(Account user)
        {
            var data = db.Accounts.SingleOrDefault(x => x.Email == user.Email);
            if (data == null)
            {
                user.GroupId = Common.CommonConstants.MEMBER_GROUP;
                db.Accounts.Add(user);
                db.SaveChanges();
                return user.Id;
            }
            else
            {
                if (data.Status == true)
                {
                    return data.Id;
                }
                else
                    return -1;
            }
        }

        public long InsertForGoogle(Account user)
        {
            var data = db.Accounts.SingleOrDefault(x => x.Email == user.Email);
            if (data == null)
            {
                user.GroupId = Common.CommonConstants.MEMBER_GROUP;
                db.Accounts.Add(user);
                db.SaveChanges();
                return user.Id;
            }
            else
            {
                return data.Id;
            }
        }

        public Account GetUserByResetPasswordCode(string resetPass)
        {
            return db.Accounts.SingleOrDefault(x => x.ResetPasswordCode == resetPass);
        }

        public double CustomerStatistic()
        {
            return db.Accounts.Where(x => x.GroupId == Common.CommonConstants.MEMBER_GROUP).Count();
        }
    }
}
