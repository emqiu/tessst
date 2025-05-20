using Data.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        TGClothesDbContext db = null;
        public OrderService()
        {
            db = new TGClothesDbContext();
        }

        public bool Delete(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        public IEnumerable<Order> GetAllByDatePaging(DateTime fromDate, DateTime toDate) /*, int page, int pageSize*/
        {
            IQueryable<Order> model = db.Orders;
            var data = model.OrderByDescending(x => x.OrderDate).ToList();
            return data.Where(x => x.OrderDate.Date >= fromDate && x.OrderDate.Date <= toDate)/*.ToPagedList(page, pageSize)*/;
        }

        public List<Order> GetAllOrderByDate(DateTime orderDate, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            var data = (from o in db.Orders
                        where o.OrderDate.Day == orderDate.Day && o.OrderDate.Month == orderDate.Month && o.OrderDate.Year == orderDate.Year && o.Status == 3
                        select o).Distinct();
            totalRecord = data.Count();
            return data.OrderByDescending(x => x.OrderDate).ToList();
        }

        public List<Order> GetAllOrderByMonth(DateTime orderDate, ref int totalRecord, int pageIndex = 1, int pageSize = 8)
        {
            var data = (from o in db.Orders
                        where o.OrderDate.Month == orderDate.Month && o.OrderDate.Year == orderDate.Year && o.Status == 3
                        select o).Distinct();
            totalRecord = data.Count();
            return data.OrderByDescending(x => x.OrderDate).ToList();
        }
        //vừa thêm hàm lấy đơn hàng theo ngày không lọc trạng thái
        public List<Order> GetOrdersByExactDate(DateTime orderDate)
        {
            return db.Orders
                .Where(o => o.OrderDate.Year == orderDate.Year
                         && o.OrderDate.Month == orderDate.Month
                         && o.OrderDate.Day == orderDate.Day)
                .ToList();
        }


        public IEnumerable<Order> GetAllPaging() /*int page, int pageSize*/
        {
            IQueryable<Order> model = db.Orders;
            return model.OrderByDescending(x => x.OrderDate)/*.ToPagedList(page, pageSize)*/;
        }

        public Order GetOrderById(long id)
        {
            return db.Orders.FirstOrDefault(x => x.Id == id);
        }

        public Order GetByOrderCode(string orderCode)
        {
            return db.Orders
                     .FirstOrDefault(o => o.OrderCode == orderCode);
        }

        public List<Order> GetOrderByUserId(long userId)
        {
            var result = (from o in db.Orders
                         join u in db.Accounts on o.CustomerId equals u.Id
                         where u.Id == userId
                         orderby o.OrderDate descending
                         select o).ToList();
            return result;
        }

        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.Id;
        }

        public double OrderStatistic()
        {
            return db.Orders.Count();
        }

        public bool Update(Order order)
        {
            try
            {
                var data = db.Orders.Find(order.Id);
                data.Name = order.Name;
                data.PaymentMethod = order.PaymentMethod;
                data.Status = order.Status;
                data.DeliveryAddress = order.DeliveryAddress;
                data.DeliveryDate = order.DeliveryDate;
                data.CustomerId = order.CustomerId;
                //data.Email = order.Email;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GenerateOrderCode()
        {
            var today = DateTime.Now.Date;
            var todayOrders = db.Orders.Where(o => o.OrderDate >= today).ToList();

            string datePart = today.ToString("yyddMM");
            int maxNumber = 0;

            foreach (var order in todayOrders)
            {
                string code = order.OrderCode;
                if (!string.IsNullOrEmpty(code) && code.StartsWith(datePart + "DH"))
                {
                    string numberPart = code.Substring((datePart + "DH").Length);
                    if (int.TryParse(numberPart, out int number))
                    {
                        if (number > maxNumber)
                            maxNumber = number;
                    }
                }
            }

            int newNumber = maxNumber + 1;
            return $"{datePart}DH{newNumber.ToString("D4")}";
        }
    }
}
