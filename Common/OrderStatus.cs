using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Common
{
    public enum OrderStatus
    {
        [Display(Name = "Chờ xử lý")]
        PENDING,

        [Display(Name = "Đang xử lý")]
        PROCESSING,

        [Display(Name = "Đang vận chuyển")]
        TRANSPORTING,

        [Display(Name = "Giao thành công")]
        SUCCESSFUL,

        [Display(Name = "Đã hủy")]
        CANCELLED
    }
}