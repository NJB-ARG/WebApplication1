using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using WebApplication1.CustomFilters;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolOperacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public RolOperacionesController()
        {
        }

        public RolOperacionesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        //Creo los managers - INI
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        //Creo los managers - FIN


        // GET: RolControladoraAccion
        public ActionResult Index()
        {
            var rolControladoraAccions = db.RolControladoraAccions.Include(r => r.accion).Include(r => r.controladora).Include(r => r.rol);
            return View(rolControladoraAccions.ToList());
        }

        // GET: RolControladoraAccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolControladoraAccion rolControladoraAccion = db.RolControladoraAccions.Find(id);
            if (rolControladoraAccion == null)
            {
                return HttpNotFound();
            }
            return View(rolControladoraAccion);
        }

        // GET: RolControladoraAccion/Create
        [UpdateControllersActions]
        public ActionResult Create()
        {
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionDescripcion");
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraDescripcion");

            ViewBag.RolID = GetRoles();

            //IEnumerable<SelectListItem> RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
            //{
            //    //Selected = userRoles.Contains(x.Name),
            //    Text = x.Name,
            //    Value = x.Name
            //});

            //ViewBag.RolID = new SelectList(RolesList, "Id", "Name");
            return View();
        }

        // POST: RolControladoraAccion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RolControladoraAccionID,RolID,ControladoraID,AccionID,OwerRequired,Activo,Inicio")] RolControladoraAccion rolControladoraAccion)
        {
            if (ModelState.IsValid)
            {
                db.RolControladoraAccions.Add(rolControladoraAccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionDescripcion", rolControladoraAccion.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraDescripcion", rolControladoraAccion.ControladoraID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolControladoraAccion.RolID);
            ViewBag.RolID = GetRoles();
            return View(rolControladoraAccion);
        }

        // GET: RolControladoraAccion/Edit/5
        [UpdateControllersActions]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolControladoraAccion rolControladoraAccion = db.RolControladoraAccions.Find(id);
            if (rolControladoraAccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionDescripcion", rolControladoraAccion.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraDescripcion", rolControladoraAccion.ControladoraID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolControladoraAccion.RolID);
            ViewBag.RolID = GetRoles();
            return View(rolControladoraAccion);
        }

        // POST: RolControladoraAccion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RolControladoraAccionID,RolID,ControladoraID,AccionID,OwerRequired,Activo,Inicio")] RolControladoraAccion rolControladoraAccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolControladoraAccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionDescripcion", rolControladoraAccion.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraDescripcion", rolControladoraAccion.ControladoraID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolControladoraAccion.RolID);
            ViewBag.RolID = GetRoles();
            return View(rolControladoraAccion);
        }

        // GET: RolControladoraAccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolControladoraAccion rolControladoraAccion = db.RolControladoraAccions.Find(id);
            if (rolControladoraAccion == null)
            {
                return HttpNotFound();
            }
            return View(rolControladoraAccion);
        }

        // POST: RolControladoraAccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RolControladoraAccion rolControladoraAccion = db.RolControladoraAccions.Find(id);
            db.RolControladoraAccions.Remove(rolControladoraAccion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public IEnumerable<SelectListItem> GetRoles()
        {

            IEnumerable<SelectListItem> RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
            {
                //Selected = userRoles.Contains(x.Name),
                Text = x.Name,
                Value = x.Name
            }
            );
            return RolesList;
        }
    }
}
