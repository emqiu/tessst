using Common;
using Data.EF;
using Data.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TGClothes.Models;

namespace TGClothes.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _userService;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductStockService _productSizeService;

        public OrderController(IOrderService orderService, 
            IAccountService userService,
            IProductService productService, 
            IOrderDetailService orderDetailService,
            IProductStockService productSizeService)
        {
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _productSizeService = productSizeService;
        }
        // GET: Admin/Order

        public ActionResult Index(DateTime? fromDate, DateTime? toDate, int? statusFilter, int page = 1, int pageSize = 8 )
        {
            //if (!fromDate.HasValue && !toDate.HasValue)
            //{
            //    var model = _orderService.GetAllPaging(page, pageSize);
            //    return View(model);
            //}
            //else
            //{
            //    var model = _orderService.GetAllByDatePaging(fromDate.Value, toDate.Value, page, pageSize);
            //    ViewBag.FromDate = fromDate;
            //    ViewBag.ToDate = toDate;
            //    return View(model);
            //}

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.StatusFilter = statusFilter;

            IEnumerable<Order> query;

            if (!fromDate.HasValue || !toDate.HasValue)
            {
                query = _orderService.GetAllPaging(); // ✅ Không truyền page/pageSize
            }
            else
            {
                query = _orderService.GetAllByDatePaging(fromDate.Value, toDate.Value); // ✅
            }

            if (statusFilter.HasValue)
            {
                //if (statusFilter.Value == (int)OrderStatus.SUCCESSFUL)
                //{
                //    query = query.Where(o => o.Status == (int)OrderStatus.SUCCESSFUL);
                //}
                //else
                //{
                //    query = query.Where(o => o.Status != (int)OrderStatus.SUCCESSFUL);
                //}
                switch (statusFilter.Value)
                {
                    case (int)OrderStatus.SUCCESSFUL:
                        query = query.Where(o => o.Status == (int)OrderStatus.SUCCESSFUL);
                        break;

                    case (int)OrderStatus.CANCELLED:
                        query = query.Where(o => o.Status == (int)OrderStatus.CANCELLED);
                        break;

                    case -1: // Đơn đang thực hiện
                        query = query.Where(o =>
                            o.Status == (int)OrderStatus.PENDING ||
                            o.Status == (int)OrderStatus.PROCESSING ||
                            o.Status == (int)OrderStatus.TRANSPORTING);
                        break;

                    default:
                        break;
                }
            }

            var model = query.OrderByDescending(x => x.OrderDate)
                             .ToPagedList(page, pageSize); // ✅ Phân trang tại đây

            return View(model);
        }

        public ActionResult Details(long id)
        {
            var data = _orderDetailService.GetOrderDetailByOrderId(id);
            var customer = from o in _orderService.GetAll()
                           join u in _userService.GetAll() on o.CustomerId equals u.Id
                           where o.Id == id
                           select new OrderCustomerModel
                           {
                               Order = o,
                               Account = u
                           };

            var detail = from p in _productService.GetAll()
                         join od in _orderDetailService.GetAll() on p.Id equals od.ProductId
                         where od.OrderId == id && p.Id == od.ProductId
                         select new OrderDetailModel
                         {
                             Product = p,
                             OrderDetail = od
                         };

            ViewBag.Customer = customer;
            ViewBag.OrderDetail = detail;
            return View(data);
        }

        public ActionResult Edit(long id)
        {
            var model = _orderService.GetOrderById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(long id, OrderStatus status, DateTime date)
        {
            var order = _orderService.GetOrderById(id);
            var orderStatus = status;
            order.Id = id;
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                TempData["Errnull"] = "Vui lòng chọn tình trạng đơn hàng!";
            }
            else
            {
                order.Status = (int)orderStatus;
                order.DeliveryDate = date;
                _orderService.Update(order);

                if (order.Status == (int)OrderStatus.CANCELLED)
                {
                    var orderDetails = _orderDetailService.GetOrderDetailByOrderId(order.Id);

                    foreach (var orderDetail in orderDetails)
                    {
                        var stock = _productSizeService.GetProductSizeByProductIdAndSizeId(orderDetail.ProductId, orderDetail.SizeId);

                        // Cộng lại số lượng tồn của sản phẩm
                        stock.Stock += orderDetail.Quantity;
                        _productSizeService.Update(stock);
                    }
                }
                return RedirectToAction("Index");
            }
            return this.Edit(id);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateStatus(int id, int status)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                if (order == null)
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });

                order.Status = status;

                // ← Thêm 2 dòng này để lưu ngày giao khi chuyển sang SUCCESSFUL
                if (status == (int)OrderStatus.SUCCESSFUL)
                    order.DeliveryDate = DateTime.Now;
                else
                    order.DeliveryDate = null;

                _orderService.Update(order);

                

                return Json(new
                {
                    success = true,
                    deliveryDate = order.DeliveryDate?.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        //[HttpGet]
        //public JsonResult GetDeliveryDate(long id)
        //{
        //    try
        //    {
        //        var order = _orderService.GetOrderById(id);
        //        if (order == null)
        //            return Json(new { success = false, message = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);

        //        var deliveryDate = order.DeliveryDate.HasValue
        //            ? order.DeliveryDate.Value.ToString("dd/MM/yyyy HH:mm:ss")
        //            : "";

        //        return Json(new { success = true, deliveryDate }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}