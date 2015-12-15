using System.Web.Mvc;

namespace Zhuang.UPMS.WebMvc.Areas.Common
{
    public class CommonAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Common";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Common_default",
                "Common/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Zhuang.UPMS.WebMvc.Areas.Common.Controllers" }//作用：使用不同域相同名的Controler不会冲突
            );
        }
    }
}