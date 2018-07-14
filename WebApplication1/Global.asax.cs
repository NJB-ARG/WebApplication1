﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Clean up Logs Table
            //ApplicationDbContext storeDB = new ApplicationDbContext();
            //foreach (var log in storeDB.ActionLogs.ToList())
            //{
            //    storeDB.ActionLogs.Remove(log);
            //}

            //storeDB.SaveChanges();
        }
    }
}