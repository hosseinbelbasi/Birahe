using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Birahe.EndPoint.ControllerAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ContestTimeAuthorizeAttribute : ActionFilterAttribute {
    private readonly string _configKey;


    public ContestTimeAuthorizeAttribute(string configKey) {
        _configKey = configKey;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        var user = context.HttpContext.User;

        // Admins always bypass
        if (user.IsInRole("admin")) {
            base.OnActionExecuting(context);
            return;
        }

        var db = (ApplicationContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationContext))!;

        var config = db.ContestConfigs.FirstOrDefault(c => c.Key == _configKey);

        if (config == null) {
            context.Result = new JsonResult(new { message = "خطا در بازیابی اطلاعات : کنترل دسترسی یافت نشد" })
                { StatusCode = 404 };
            return;
        }

        if (DateTime.UtcNow < config.StartTime) {
            context.Result = new JsonResult(new { message = $"{config.message}  هنوز شروع نشده است !" })
                { StatusCode = 403 };
            return;
        }

        if (DateTime.UtcNow > config.EndTime) {
            context.Result = new JsonResult(new { message = $"{config.message} تمام شده است !" })
                { StatusCode = 403 };
            return;
        }

        base.OnActionExecuting(context);
    }
}