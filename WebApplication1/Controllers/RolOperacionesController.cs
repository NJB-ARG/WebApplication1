using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repository;
using WebApplication1.Service;
using Microsoft.AspNet.Identity.Owin;
using WebApplication1.CustomFilters;
//using System.Threading.Tasks;
//using Microsoft.Owin;
//using Microsoft.Owin.Security;
using System.Web.UI.WebControls;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolOperacionesController : Controller
    {

        //INI-service-repository
        private IidentityService _service;

        public RolOperacionesController(IidentityService service)
        {
            _service = service;
        }
        //FIN-service-repository

        private ApplicationDbContext db = new ApplicationDbContext();

        public RolOperacionesController()
        {
            _service = new IdentityService(this.ModelState, new IdentityRepository());
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

        //ApplicationDbContext _dbContext;
        //public ApplicationDbContext DbOwinContext
        //{
        //    get
        //    {
        //        return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //    }
        //    private set
        //    {
        //        _dbContext = value;
        //    }
        //}
        //Creo los managers - FIN    

        // GET: RolOperaciones
        public ActionResult Index(int? id, int? controladoraID)
        {
                      
            //var rolOperaciones = db.RolOperaciones.Include(r => r.RolOperaciones_Operacion).Include(r => r.RolOperaciones_Rol);            
            
            //return View(rolOperaciones.ToList());

            var viewModel = new RolOperacionesIndexViewModel();
            //viewModel.RolOperaciones = db.RolOperaciones.Include(r => r.RolOperaciones_Operacion).Include(r => r.RolOperaciones_Rol);                        

            var roloperaciones = db.RolOperaciones            
            .Include(r => r.RolOperaciones_Operacion.Operacion_Controladora)            
            .Include(r => r.RolOperaciones_Operacion.Operacion_Accion)
            .Include(r => r.RolOperaciones_Rol);

            foreach (RolOperaciones e in roloperaciones)
            {
                var rol_operacion_operacion = db.Operaciones.Where(r => r.OperacionID == e.OperacionID).ToList().SingleOrDefault();
                e.RolOperaciones_Operacion = rol_operacion_operacion;
                e.RolOperaciones_Operacion.OperacionID = rol_operacion_operacion.OperacionID;
                if (rol_operacion_operacion != null)
                {
                    e.OperacionID = rol_operacion_operacion.OperacionID;

                    var rol_operacion_controladora = db.Controladoras.Where(r => r.ControladoraID == rol_operacion_operacion.ControladoraID).ToList().SingleOrDefault();
                    var rol_operacion_accion = db.Acciones.Where(r => r.AccionID == rol_operacion_operacion.AccionID).SingleOrDefault();

                    if (rol_operacion_controladora == null || rol_operacion_accion == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    e.RolOperaciones_Operacion.Operacion_Controladora.ControladoraID = rol_operacion_controladora.ControladoraID;
                    e.RolOperaciones_Operacion.Operacion_Controladora = rol_operacion_controladora;
                    e.RolOperaciones_Operacion.Operacion_Accion.AccionID = rol_operacion_accion.AccionID;
                    e.RolOperaciones_Operacion.Operacion_Accion = rol_operacion_accion;
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var rol_operacion_rol = db.ApplicationRoles.Where(r => r.Id == e.RolID).ToList().SingleOrDefault();
                e.RolOperaciones_Rol = rol_operacion_rol;
                e.RolOperaciones_Rol.Id = rol_operacion_rol.Id;
            }

            viewModel.RolOperaciones = roloperaciones;

            viewModel.Modulos = db.Modulos.Include(i => i.Modulo_Controladoras).OrderBy(i => i.ModuloDescripcion);                       

            //id de modulo
            if (id != null)
            {
                ViewBag.ModuloID = id.Value;
                viewModel.Controladoras = viewModel.Modulos.Where(i => i.ModuloID == id.Value).Single().Modulo_Controladoras;
            }

            if (controladoraID != null)
            {
                ViewBag.ControladoraID = controladoraID.Value;
                viewModel.Operaciones = viewModel.Controladoras.Where(x => x.ControladoraID == controladoraID).Single().Controladora_Operaciones;                
            }

            return View(viewModel);
        }

        // GET: RolOperaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperaciones rolOperaciones = db.RolOperaciones.Find(id);
            if (rolOperaciones == null)
            {
                return HttpNotFound();
            }
            return View(rolOperaciones);
        }

        // GET: RolOperaciones/Create
        [UpdateControllersActions]
        public ActionResult Create()
        {
            ViewBag.OperacionID = new SelectList(db.Operaciones, "OperacionID", "OperacionDescripcion");
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name");
            ViewBag.RolID = GetRoles("");
            return View();
        }

        // POST: RolOperaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RolOperacionesID,RolOperacionesOwerRequired,RolOperacionesActivo,RolID,OperacionID")] RolOperaciones rolOperaciones)
        {
            if (ModelState.IsValid)
            {
                //if (rolOperaciones.RolID == null || rolOperaciones.OperacionID == 0)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}

                //                
                //var rol = RoleManager.Roles.Where(s => s.Id == rolOperaciones.RolID).SingleOrDefault();
                //var role = await RoleManager.FindByIdAsync(rolOperaciones.RolID);
                //**var role = await RoleManager.FindByNameAsync(rolOperaciones.RolID);
                //**rolOperaciones.RolOperaciones_Rol = role;                
                //rolOperaciones.RolOperaciones_Rol.Name = rolOperaciones.RolID;

                //RolOperaciones rolOperacionesToInsert = new RolOperaciones();
                //rolOperacionesToInsert = rolOperaciones;
                //rolOperacionesToInsert.RolOperaciones_Rol = GetRole(rolOperaciones.RolID);
                //**var operacion = DbOwinContext.Operaciones.Where(s => s.OperacionID == rolOperaciones.OperacionID).SingleOrDefault();
                //**rolOperaciones.RolOperaciones_Operacion = operacion;
                //rolOperacionesToInsert.RolOperaciones_Operacion = operacion;

                //AsNoTracking().                                                

                //**DbOwinContext.RolOperaciones.Add(rolOperaciones);
                //**DbOwinContext.SaveChanges();
                rolOperaciones.RolOperaciones_Operacion = null;
                rolOperaciones.RolOperaciones_Rol = null;

                db.RolOperaciones.Add(rolOperaciones);
                //db.RolOperaciones.Add(rolOperacionesToInsert); 
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OperacionID = new SelectList(db.Operaciones, "OperacionID", "OperacionDescripcion", rolOperaciones.OperacionID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolOperaciones.RolID);
            ViewBag.RolID = GetRoles(rolOperaciones.RolID);
            return View(rolOperaciones);
        }

        // GET: RolOperaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperaciones rolOperaciones = db.RolOperaciones.Find(id);
            if (rolOperaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.OperacionID = new SelectList(db.Operaciones, "OperacionID", "OperacionDescripcion", rolOperaciones.OperacionID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolOperaciones.RolID);
            ViewBag.RolID = GetRoles(rolOperaciones.RolID);
            return View(rolOperaciones);
        }

        // POST: RolOperaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RolOperacionesID,RolOperacionesOwerRequired,RolOperacionesActivo,RolID,OperacionID")] RolOperaciones rolOperaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOperaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OperacionID = new SelectList(db.Operaciones, "OperacionID", "OperacionDescripcion", rolOperaciones.OperacionID);
            //ViewBag.RolID = new SelectList(db.IdentityRoles, "Id", "Name", rolOperaciones.RolID);
            ViewBag.RolID = GetRoles(rolOperaciones.RolID);
            return View(rolOperaciones);
        }

        // GET: RolOperaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOperaciones rolOperaciones = db.RolOperaciones.Find(id);
            if (rolOperaciones == null)
            {
                return HttpNotFound();
            }
            return View(rolOperaciones);
        }

        // POST: RolOperaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RolOperaciones rolOperaciones = db.RolOperaciones.Find(id);
            db.RolOperaciones.Remove(rolOperaciones);
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

        public IEnumerable<SelectListItem> GetRoles(string rol)
        {

            IEnumerable<SelectListItem> RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
            {
                //Selected = userRoles.Contains(x.Name),
                Selected = x.Name == rol,
                Text = x.Name,
                Value = x.Id
            }
            );

            //foreach (SelectListItem item in RolesList)
            //{
            //    if (item.Text == rol)
            //    {
            //        item.Selected = true;
            //    }                  
            //}            
            return RolesList;
        }

        public ApplicationRole GetRole(string rolID)
        {

            var rol = RoleManager.Roles.Where(s => s.Name == rolID).SingleOrDefault();
            
            return rol;
        }

    }
}
