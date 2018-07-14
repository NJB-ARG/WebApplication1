using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;


namespace WebApplication1.HtmlHelperExtensions
{
    public class CustomAssemblyHelper
    {
        //TODOS
        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public List<Type> GetTypesAssembly<T>()
        {
            return GetSubClasses<T>();
        }

        //UNA instancia en particular
        private static Type GetSubClassesInstancia<T>(string instancia)
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T)) && type.Name == instancia).Single();
        }

        public Type GetTypesAssemblyInstancia<T>(string instancia)
        {
            return GetSubClassesInstancia<T>(instancia);
        }

        //NJB-INI
        public List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }        

        public IEnumerable<Attribute> GetTypeAttributes<T>(Type tipo)
        {
            return tipo.GetCustomAttributes(typeof(T));
        }
        public IEnumerable<Attribute> GetTypeAttributes<T>(MethodInfo metodo)
        {
            return metodo.GetCustomAttributes(typeof(T));
        }

        public IEnumerable<MethodInfo> GetControllerActions(Type tipo)
        {
            return tipo.GetMethods().Where(t => t.Name != "Dispose" && !t.IsSpecialName && t.DeclaringType.IsSubclassOf(typeof(ControllerBase)) && t.IsPublic && !t.IsStatic).ToList();
        }
        //NJB-FIN

        //public static List<String> GetControllerActions()
        //{
        //    List<String> controllerActions = new List<String>();

        //    foreach (Type controller in CustomAssemblyHelper.GetSubClasses<Controller>())
        //    {
        //        if (controller.Name != "HomeController" && controller.Name != "AccountController")
        //        {
        //            var atributos = controller.GetCustomAttributes(typeof(MenuItemAttribute));
        //            MenuItemAttribute mia = (MenuItemAttribute)atributos.FirstOrDefault();
        //            string controllerName = controller.Name.Replace("Controller", "");
        //            var actions = controller.GetMethods().Where(t => t.Name != "Dispose" && !t.IsSpecialName && t.DeclaringType.IsSubclassOf(typeof(ControllerBase)) && t.IsPublic && !t.IsStatic).ToList();
        //            foreach (var action in actions)
        //            {
        //                var myAttributes = action.GetCustomAttributes(false);
        //                for (int j = 0; j < myAttributes.Length; j++)
        //                    if (myAttributes[j].GetType().Name == "AuthorizeAttribute")
        //                    {
        //                        var descripcion = action.GetCustomAttributes(typeof(ActionAttribute));
        //                        ActionAttribute desc = (ActionAttribute)descripcion.FirstOrDefault();
        //                        //controllerActions.Add(new Models.Operacion(mia.Modulo, mia.Nombre, desc.Descripcion, controllerName, action.Name, desc.Inicial));
        //                        //controllerNames.Add(mia.Modulo + " :: " + mia.Nombre + " :: " + controller.Name + " :: " + action.Name + " :: " + desc.Descripcion);
        //                    }
        //            }
        //        }
        //    }
        //    return controllerActions;
        //}
    }

    //Acciones
    public class ActionAttribute : Attribute
    {
        public double AccionNumero  { get; set; }
        public string AccionNombre { get; set; }
        public string AccionDescripcion { get; set; }
        //valor que indica si al inicio se carga como ACTIVO o INACTIVO - INSERT en operaciones para cada AccionInicial=TRUE
        public bool AccionInicial { get; set; }
    }

    //Controladoras
    public class MenuItemAttribute : Attribute
    {
        public double MenuItemNumero { get; set; }
        public string MenuItemDescripcion { get; set; }
        //para insertar en tabla modulos
        public string MenuItemModulo { get; set; }
    }

    public static class HtmlRequestHelper
    {
        public static string Id(this HtmlHelper htmlHelper)
        {
            var routeValues = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("id"))
                return (string)routeValues["id"];
            else if (HttpContext.Current.Request.QueryString.AllKeys.Contains("id"))
                return HttpContext.Current.Request.QueryString["id"];

            return string.Empty;
        }

        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string Action(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }

        public static string User(this HtmlHelper htmlHelper)
        {
            var user = HttpContext.Current.User.Identity.Name;            

            if (!string.IsNullOrEmpty(user))
                return user;

            return string.Empty;
        }
    }
}