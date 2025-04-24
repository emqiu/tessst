using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Data.Services
{
    public interface IAccountService
    {
        long Insert(Account user);
        long InsertForFacebook(Account user);
        long InsertForGoogle(Account user);
        bool Update(Account user);
        IEnumerable<Account> GetAllPaging(string searchString, int page, int pageSize);
        List<Account> GetAll();
        Account GetUserById(long id);
        Account GetUserByEmail(string email);
        Account GetUserByResetPasswordCode(string resetPass);
        bool Delete(long id);
        int Login(string userName, string password);
        int LoginByEmail(string email, string password, bool isLoginAdmin);
        //List<string> GetListCredential(string email);
        bool ChangeStatus(long id);
        bool CheckEmailExist(string email);
        double CustomerStatistic();
    }
}
