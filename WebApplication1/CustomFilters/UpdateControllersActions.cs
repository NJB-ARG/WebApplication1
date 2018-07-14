using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.HtmlHelperExtensions;
using System.Data.Entity.Migrations;

namespace WebApplication1.CustomFilters
{
    public class UpdateControllersActions : ActionFilterAttribute, IActionFilter
    {
        //OnActionExecuting method is using Entity Framework to add a new ActionLog register. It creates and fills a new entity instance with the context information from filterContext.
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Log Action Filter Call
            ApplicationDbContext db = new ApplicationDbContext();
            
            // TODO: Add your acction filter's tasks here           
            CustomAssemblyHelper helper = new CustomAssemblyHelper();
            var syscontrollersList = helper.GetTypesAssembly<Controller>();           

            foreach (Type controller in syscontrollersList)
            {               
                //if (controller.Name != "HomeController" && controller.Name != "AccountController" && controller.Name != "PerfilController" && controller.Name != "RolesAdminController" && controller.Name != "UsersAdminController")                
                if (controller.Name == "SolicitudController" || controller.Name == "OrdenController")
                {
                    string controladoraNombre = controller.Name.Replace("Controller", "");

                    if (controller.IsDefined(typeof(MenuItemAttribute), false))
                    {
                        var controladoraAtributos = helper.GetTypeAttributes<MenuItemAttribute>(controller);
                        MenuItemAttribute atributomenu = (MenuItemAttribute)controladoraAtributos.FirstOrDefault();

                        var moduloToInsert = new Modulo();
                        moduloToInsert.ModuloDescripcion = atributomenu.MenuItemModulo;

                        //db.Modulos.AddOrUpdate(p => p.ModuloDescripcion, moduloToInsert);
                        var moduloInDataBase = db.Modulos.Where(s => s.ModuloDescripcion == moduloToInsert.ModuloDescripcion).SingleOrDefault();
                        if (moduloInDataBase == null)
                        {
                            db.Modulos.Add(moduloToInsert);
                            db.SaveChanges();
                        }                        

                        Controladora controladoratoinsert = new Controladora()
                        {
                            ControladoraNumero = atributomenu.MenuItemNumero,
                            ControladoraModulo = atributomenu.MenuItemModulo,
                            ControladoraNombre = controladoraNombre,
                            ControladoraNombreFull = controller.Name,
                            ControladoraDescripcion = atributomenu.MenuItemDescripcion,
                            Controladora_Modulo = moduloToInsert
                        };
                        
                        //db.Controladoras.AddOrUpdate(p => p.ControladoraNombre, controladoratoinsert);
                        var controladoraInDataBase = db.Controladoras.Where(s => s.ControladoraNombre == controladoratoinsert.ControladoraNombre).SingleOrDefault();
                        if (controladoraInDataBase == null)
                        {
                            db.Controladoras.Add(controladoratoinsert);
                            db.SaveChanges();
                        }

                        //Recupero todas las acciones de la controladorapor REFLECTION
                        var accionescontroladora = helper.GetControllerActions(controller);

                        foreach (var action in accionescontroladora)
                        {
                            var accionAtributos = helper.GetTypeAttributes<ActionAttribute>(action);
                            ActionAttribute accionatributo = (ActionAttribute)accionAtributos.FirstOrDefault();

                            //foreach (var atributo in accionatributos)
                            //{
                            //    if (atributo.GetType().Name == "ActionAttribute")
                            //    {
                            //        var descripcion = action.GetCustomAttributes(typeof(ActionAttribute));
                            //        ActionAttribute desc = (ActionAttribute)descripcion.FirstOrDefault();
                            //    }
                            //}
                            if (action.IsDefined(typeof(ActionAttribute), false) && accionatributo != null)
                            { 
                                Accion acciontoinsert = new Accion()
                                {
                                    AccionNumero = accionatributo.AccionNumero,
                                    AccionActivo = accionatributo.AccionInicial,
                                    AccionNombre = action.Name,
                                    AccionDescripcion = accionatributo.AccionNombre,                            
                                };

                                //db.Acciones.AddOrUpdate(p => p.AccionNombre, acciontoinsert);
                                var accionInDataBase = db.Acciones.Where(s => s.AccionNombre == acciontoinsert.AccionNombre).SingleOrDefault();
                                if (accionInDataBase == null)
                                {
                                    db.Acciones.Add(acciontoinsert);
                                    db.SaveChanges();                                    
                                }
                                
                            }                            
                        }                        
                    }
                }                
            }                       

            this.OnActionExecuting(filterContext);
        }

        //controladorasToInsert.ForEach(s => db.Controladoras.AddOrUpdate(p => p.ControladoraNombre, s));

        //controladoras.ForEach(s => db.Controladora.AddOrUpdate(p => p.LastName, s));
        //db.SaveChanges();

        //foreach (Enrollment e in enrollments)
        //{
        //    var enrollmentInDataBase = context.Enrollments.Where(
        //        s =>
        //             s.Student.ID == e.StudentID &&
        //             s.Course.CourseID == e.CourseID).SingleOrDefault();
        //    if (enrollmentInDataBase == null)
        //    {
        //        context.Enrollments.Add(e);
        //    }
        //}
        //context.SaveChanges();
    }
}


//USE
//[MyNewCustomActionFilter(Order = 2)]
//[CustomActionFilter(Order = 1)]
//public class StoreController : Controller
//{
//...
//}


//si la controladora todavía no se inserto, se inserta
//if (!db.Controladoras.Any(u => u.ControladoraNombre == atributomenu.MenuItemNombre && u.ControladoraNumero == atributomenu.MenuItemNumero))
//{
//    db.Controladoras.Add(controladoratoinsert);
//    db.SaveChanges();
//}