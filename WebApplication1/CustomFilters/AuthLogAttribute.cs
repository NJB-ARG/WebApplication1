using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace WebApplication1.CustomFilters
{
    public class AuthLogAttribute : AuthorizeAttribute
    {
        public AuthLogAttribute()
        {
            View = "AuthorizeFailed";
        }

        public string View { get; set; }

        // <summary>
        // Check for Authorization
        // </summary>
        // <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        // <summary>
        // Method to check if the user is Authorized or not
        // if yes continue to perform the action else redirect to error page
        // </summary>
        // <param name="filterContext"></param>
        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.IsInRole("Admin"))
            {
                return;
            }

            // If the Result returns null then the user is Authorized 
            if (filterContext.Result == null)
            {                

                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                using (var context = new ApplicationDbContext())
                {
                    var OperacionRoles = context.Perfiles.Where(i => i.Perfil_Accion.AccionNombre == actionName && i.Perfil_Controladora.ControladoraNombre == controllerName);

                    foreach (var perfil in OperacionRoles)
                    {
                        if (filterContext.HttpContext.User.IsInRole(perfil.Perfil_Rol.Name))
                        {
                            return;
                        }
                    }
                    
                }
                // auth failed, redirect to login page
                //filterContext.Result = new HttpUnauthorizedResult();               
            }

            //If the user is Un-Authorized then Navigate to Auth Failed View 
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                // var result = new ViewResult { ViewName = View };
                var vr = new ViewResult();
                vr.ViewName = View;
                
                ViewDataDictionary dict = new ViewDataDictionary();
                dict.Add("Message", "Sorry you are not Authorized to Perform this Action");

                vr.ViewData = dict;

                var result = vr;

                filterContext.Result = result;
            }
        }
    }
}