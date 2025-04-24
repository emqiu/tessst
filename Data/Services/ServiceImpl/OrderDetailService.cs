using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.ServiceImpl
{
    public class OrderDetailService : IOrderDetailService
    {
        TGClothesDbContext db = null;
        public OrderDetailService()
        {
            db = new TGClothesDbContext();
        }

        public List<OrderDetail> GetAll()
        {
            return db.OrderDetails.ToList();
        }

        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<OrderDetail> GetOrderDetailByOrderId(long id)
        {
            return (from od in db.OrderDetails
                    join o in db.Orders on od.OrderId equals o.Id
                    where o.Id == id
                    select od).ToList();
        }

        public bool Delete(OrderDetail orderDetail)
        {
            try
            {
                db.OrderDetails.Remove(orderDetail);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public decimal TotalRevenue()
        {
            var result = from o in db.Orders
                         join od in db.OrderDetails on o.Id equals od.OrderId
                         where o.Status == 3
                         select od;
            return result.Sum(x => x.TotalPrice).Value;
        }

        public decimal MonthlyRevenue()
        {
            var result = (from o in db.Orders
                         join od in db.OrderDetails on o.Id equals od.OrderId
                         where o.Status == 3 && o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year
                         select od).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Key.TotalPrice).HasValue ? result.Sum(x => x.Key.TotalPrice).Value : 0;
        }

        public decimal AnnualRevenue()
        {
            var result = (from o in db.Orders
                         join od in db.OrderDetails on o.Id equals od.OrderId
                         where o.Status == 3 && o.OrderDate.Year == DateTime.Now.Year
                         select od).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Key.TotalPrice).HasValue ? result.Sum(x => x.Key.TotalPrice).Value : 0;
        }

        public decimal DailyRevenue()
        {
            var result = (from o in db.Orders
                          join od in db.OrderDetails on o.Id equals od.OrderId
                          where o.Status == 3 && o.OrderDate.Day == DateTime.Now.Day && o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year
                          select od).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Key.TotalPrice).HasValue ? result.Sum(x => x.Key.TotalPrice).Value : 0;
        }

        public decimal DailyProfit()
        {
            var result = (from o in db.Orders
                          join od in db.OrderDetails on o.Id equals od.OrderId
                          join p in db.Products on od.ProductId equals p.Id
                          where o.Status == 3 && o.OrderDate.Day == DateTime.Now.Day && o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year
                          select new { 
                            od.OrderId,
                            od.TotalPrice,
                            od.ProductId,
                            od.Quantity,
                            p.OriginalPrice
                          }).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).HasValue ? result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).Value : 0;
        }

        public decimal MonthlyProfit()
        {
            var result = (from o in db.Orders
                          join od in db.OrderDetails on o.Id equals od.OrderId
                          join p in db.Products on od.ProductId equals p.Id
                          where o.Status == 3 &&  o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year
                          select new
                          {
                              od.OrderId,
                              od.TotalPrice,
                              od.ProductId,
                              od.Quantity,
                              p.OriginalPrice
                          }).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).HasValue ? result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).Value : 0;
        }

        public decimal AnnualProfit()
        {
            var result = (from o in db.Orders
                          join od in db.OrderDetails on o.Id equals od.OrderId
                          join p in db.Products on od.ProductId equals p.Id
                          where o.Status == 3 && o.OrderDate.Year == DateTime.Now.Year
                          select new
                          {
                              od.OrderId,
                              od.TotalPrice,
                              od.ProductId,
                              od.Quantity,
                              p.OriginalPrice
                          }).GroupBy(x => new { x.OrderId, x.TotalPrice });
            return result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).HasValue ? result.Sum(x => x.Sum(item => item.TotalPrice - item.OriginalPrice * item.Quantity)).Value : 0;
        }
    }
}