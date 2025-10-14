using Birahe.EndPoint.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Birahe.EndPoint.ControllerAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ContestTimeAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string _configKey;

    // 👇 Parameter identifies which start time this attribute should use
    public ContestTimeAuthorizeAttribute(string configKey)
    {
        _configKey = configKey;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        // Admins always bypass
        if (user.IsInRole("admin"))
        {
            base.OnActionExecuting(context);
            return;
        }

        var db = (ApplicationContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationContext))!;

        // Each configKey could represent a different contest phase
        var config = db.ContestConfigs.FirstOrDefault(c => c.Key == _configKey);
        if (config == null)
        {
            context.Result = new JsonResult(new { message = "خطا در بازیابی اطلاعات : کنترل دسترسی یافت نشد" }) { StatusCode = 404 };
            return;
        }

        if (DateTime.UtcNow < config.StartTime)
        {
            context.Result = new JsonResult(new { message = "مسابقه هنوز شروع نشده است !" })
                { StatusCode = 403 };
            return;
        }

        base.OnActionExecuting(context);
    }
}