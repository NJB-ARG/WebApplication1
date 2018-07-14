using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using System.Data.Common;
using System.Data.Linq;
using WebApplication1.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.CustomFilters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RoleOrOwnerAuthorizationAttribute : AuthorizeAttribute
    {
        private IDataContextFactory ContextFactory { get; set; }

        private string routeParameter = "id";
        /// <summary>
        /// The name of the routing parameter to use to identify the owner of the data (user id) in question.  Default is "id".
        /// </summary>
        public string RouteParameter
        {
            get { return this.routeParameter; }
            set { this.routeParameter = value; }
        }

        public RoleOrOwnerAuthorizationAttribute() : this(null)
        {
        }

        public RoleOrOwnerAuthorizationAttribute(IDataContextFactory factory)
        {
            this.ContextFactory = factory ?? new MyDataContextFactory();
        }

        protected virtual void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual void SetCachePolicy(AuthorizationContext filterContext)
        {
            // ** IMPORTANT **
            // Since we're performing authorization at the action level, the authorization code runs
            // after the output caching module. In the worst case this could allow an authorized user
            // to cause the page to be cached, then an unauthorized user would later be served the
            // cached page. We work around this by telling proxies not to cache the sensitive page,
            // then we hook our custom authorization code into the caching mechanism so that we have
            // the final say on whether a page should be served from the cache.
            HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                SetCachePolicy(filterContext);
            }
            else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // auth failed, redirect to login page
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else if (filterContext.HttpContext.User.IsInRole("Admin") || IsOwner(new CurrentRequest(filterContext, this.RouteParameter)))
            {
                SetCachePolicy(filterContext);
            }
            else
            {
                ViewDataDictionary viewData = new ViewDataDictionary();
                viewData.Add("Message", "You do not have sufficient privileges for this operation.");
                filterContext.Result = new ViewResult { ViewName = "Error", ViewData = viewData };
            }

        }

        private bool IsOwner(CurrentRequest requestData)
        {
            using (var dc = this.ContextFactory.GetDataContextWrapper())
            {
                return dc.Table<IdentityUser>().Where(p => p.UserName == requestData.Username && p.Id == requestData.OwnerID).Any();
            }
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            var status = base.OnCacheAuthorization(httpContext);
            if (status == HttpValidationStatus.IgnoreThisRequest && IsOwner(new CurrentRequest(httpContext, this.RouteParameter)))
            {
                status = HttpValidationStatus.Valid;
            }
            return status;
        }
    }



    public interface IDataContextFactory
    {
        IDataContextWrapper GetDataContextWrapper();
    }


    public class MyDataContextFactory : IDataContextFactory
    {

        //private static readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog = WebApplication1Context - 20160711234937; Integrated Security = True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|WebApplication1Context-20160711234937.mdf";
        private static readonly string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-WebApplication1-20160627012039.mdf;Initial Catalog=aspnet-WebApplication1-20160627012039;Integrated Security=True";
        public virtual IDataContextWrapper GetDataContextWrapper()
        {            

            DataContext MyDataContext = new DataContext(connectionString);
            return new DataContextWrapper(MyDataContext);            
        }
    }

    public interface IDataContextWrapper : IDisposable
    {
        void DeleteOnSubmit<T>(T entity) where T : class;
        void DeleteAllOnSubmit<T>(IEnumerable<T> entities) where T : class;
        void InsertOnSubmit<T>(T entity) where T : class;
        void InsertAllOnSubmit<T>(IEnumerable<T> entities) where T : class;
        void SubmitChanges();
        IQueryable<T> Table<T>() where T : class;
        IQueryable<T> Query<T>() where T : class;
        IEnumerable<TResult> ExecuteQuery<TResult>(string query, params object[] parameters);
        DbConnection Connection();
    }



    public class CurrentRequest
    {

        public CurrentRequest()
        {
            this.OwnerID = "-1";
        }

        public CurrentRequest(AuthorizationContext filterContext, string routeParameter)
            : this()
        {
            if (filterContext.RouteData.Values.ContainsKey(routeParameter))
            {
                //this.OwnerID = Convert.ToInt32(filterContext.RouteData.Values[routeParameter]);
                this.OwnerID = Convert.ToString(filterContext.RouteData.Values[routeParameter]);
            }

            this.Username = filterContext.HttpContext.User.Identity.Name;
        }

        public CurrentRequest(HttpContextBase httpContext, string routeParameter)
        {
            if (!string.IsNullOrEmpty(routeParameter))
            {
                if (httpContext.Request.Params[routeParameter] != null)
                {
                    this.OwnerID = GetOwnerID(httpContext.Request.Params[routeParameter]);
                }
                else if (string.Equals("id", routeParameter, StringComparison.OrdinalIgnoreCase))
                {
                    // id may be last element in path if not included as a parameter
                    this.OwnerID = GetOwnerID(httpContext.Request.Path.Split('/').Last());
                }
            }

            this.Username = httpContext.User.Identity.Name;
        }

        private string GetOwnerID(string id)
        {
            //int ownerID;
            //if (!int.TryParse(id, out ownerID))
            //{
            //    ownerID = -1;
            //}
            string ownerID = id;
            return ownerID;
        }

        //public int OwnerID { get; set; }
        public string OwnerID { get; set; }
        public string Username { get; set; }
    }


    ////////////
    public class DataContextWrapper : IDataContextWrapper
    {
        protected DataContext Db;        

        public DataContextWrapper(DataContext context)
        {         
            this.Db = context;
        }

        //public DataContextWrapper(DataContext context, DataContextConfig conn)
        //{
        //    context.Connection.ConnectionString = conn.ConnectionString;
        //    this.Db = context;
        //}

        public IQueryable<T> Table<T>() where T : class
        {
            return Db.GetTable<T>().AsQueryable();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Db.GetTable<T>().AsQueryable();
        }

        public void DeleteOnSubmit<T>(T entity) where T : class
        {
            Db.GetTable<T>().DeleteOnSubmit(entity);
        }

        public void DeleteAllOnSubmit<T>(IEnumerable<T> entities) where T : class
        {
            Db.GetTable<T>().DeleteAllOnSubmit(entities);
        }

        public void InsertOnSubmit<T>(T entity) where T : class
        {
            Db.GetTable<T>().InsertOnSubmit(entity);
        }

        public void InsertAllOnSubmit<T>(IEnumerable<T> entities) where T : class
        {
            Db.GetTable<T>().InsertAllOnSubmit(entities);
        }

        public void SubmitChanges()
        {
            try
            {
                Db.SubmitChanges();
            }
            //catch (Exception ex)
            catch
            {          
            }
        }

        public IEnumerable<TResult> ExecuteQuery<TResult>(string query, params object[] parameters)
        {
            return Db.ExecuteQuery<TResult>(query, parameters);
        }

        public DbConnection Connection()
        {
            return Db.Connection;
        }

        void IDisposable.Dispose()
        {

        }

    }

}