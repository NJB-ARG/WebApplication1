using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;


namespace WebApplication1.CustomFilters
{
    public class TrazabilidadDatos : ActionFilterAttribute, IActionFilter
    {
        //OnActionExecuting method is using Entity Framework to add a new ActionLog register. It creates and fills a new entity instance with the context information from filterContext.
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            // TODO: Add your action filter's tasks here

            // Log Action Filter call
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ActionLog log = new ActionLog()
                {
                    Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Trazabilidad Action Filter)"),
                    IP = filterContext.HttpContext.Request.UserHostAddress,
                    DateTime = filterContext.HttpContext.Timestamp
                };
                db.ActionLogs.Add(log);
                db.SaveChanges();
                OnActionExecuting(filterContext);
            }
        }
    }
}


//When a filter is injected into a controller class, all its actions are also injected
//If you would like to apply the filter only for a set of actions, you would have to inject[CustomActionFilter] to each one of them

//You can define a Scope for each of the Filters, for example, you could scope all the Action Filters to run within the Controller Scope, and all Authorization Filters to run in Global scope
//The scopes have a defined execution order.+
//Additionally, each action filter has an Order property which is used to determine the execution order in the scope of the filter.
//For more information about Custom Action Filters execution order, please visit this MSDN article: (https://msdn.microsoft.com/library/dd381609(v=vs.98).aspx).