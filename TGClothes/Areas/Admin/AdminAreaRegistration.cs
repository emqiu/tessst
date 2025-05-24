using System.Web.Mvc;

namespace TGClothes.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Định tuyến riêng cho trang hỗ trợ
            context.MapRoute(
                name: "Admin_Chat_Support",
                url: "Admin/chatroom/support",
                defaults: new { controller = "Chat", action = "Support" },
                namespaces: new[] { "TGClothes.Areas.Admin.Controllers" }
            );

            // Định tuyến chung cho Admin
            var route = context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TGClothes.Areas.Admin.Controllers" }
            );

            // Sử dụng DataTokens để chỉ định chính xác "Admin"
            route.DataTokens["area"] = "Admin";
        }
    }
}