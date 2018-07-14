using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.CustomFilters;
using WebApplication1.HtmlHelperExtensions;

namespace WebApplication1.Controllers
{
    public class PerfilController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Perfil
        public ActionResult Index(int? id, int? controladoraID)
        {
            //var perfiles = db.Perfiles.Include(p => p.Perfil_Accion).Include(p => p.Perfil_Controladora).Include(p => p.Perfil_Rol);                        
            //return View(perfiles.ToList());

            var viewModel = new PerfilIndexViewModel();
            viewModel.Perfiles = db.Perfiles.Include(p => p.Perfil_Accion).Include(p => p.Perfil_Controladora).Include(p => p.Perfil_Rol);            

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
                string controladoraNombreFull = viewModel.Controladoras.Where(i => i.ControladoraID == controladoraID).Single().ControladoraNombreFull;
                CustomAssemblyHelper helper = new CustomAssemblyHelper();
                var syscontroller = helper.GetTypesAssemblyInstancia<Controller>(controladoraNombreFull);

                //Recupero todas las acciones de la controladora por REFLECTION
                var accionescontroladorasys = helper.GetControllerActions(syscontroller).ToList();
                var accionesSYS = accionescontroladorasys.Select(r => r.Name).Distinct().ToList();
                var accionesDB = db.Acciones;
                List<Accion> accionescontroladora = new List<Accion>();  
                foreach (var accionDB in accionesDB)
                {
                    foreach (var acionSYS in accionesSYS)
                    {
                        if (accionDB.AccionNombre == acionSYS)
                        {
                            accionescontroladora.Add(accionDB);
                        }
                    }
                }                                

                viewModel.Acciones = accionescontroladora;
            }

            return View(viewModel);
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfiles.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // GET: Perfil/Create
        [UpdateControllersActions]
        public ActionResult Create()
        {
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionNombre");
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraNombre");
            //ViewBag.RolID = new SelectList(db.Roles, "Id", "Name");
            ViewBag.RolID = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        // POST: Perfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PerfilID,PerfilDescripcion,PerfilInicial,PerfilOwerRequerido,ControladoraID,AccionID,RolID")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(perfil.Perfil_Rol).State = EntityState.Unchanged;
                //perfil.Perfil_Rol = null;

                string controladoraNombreFull = db.Controladoras.Where(i => i.ControladoraID == perfil.ControladoraID).Single().ControladoraNombreFull;
                CustomAssemblyHelper helper = new CustomAssemblyHelper();
                var syscontroller = helper.GetTypesAssemblyInstancia<Controller>(controladoraNombreFull);

                //Recupero todas las acciones de la controladorapor REFLECTION
                var accionescontroladora = helper.GetControllerActions(syscontroller);
                var nombreAccion = db.Acciones.Where(r => r.AccionID == perfil.AccionID).Single().AccionNombre;
                var action = accionescontroladora.Where(r => r.Name == nombreAccion).Single();
                var accionAtributos = helper.GetTypeAttributes<ActionAttribute>(action);
                ActionAttribute accionatributo = (ActionAttribute)accionAtributos.FirstOrDefault();
                var nombreRol = db.Roles.Where(r => r.Id == perfil.RolID).Single().Name;

                var controladoraDesc = db.Controladoras.Where(i => i.ControladoraID == perfil.ControladoraID).Single().ControladoraDescripcion;
                //var accionDesc = db.Acciones.Where(i => i.AccionID == perfil.AccionID).Single().AccionDescripcion;
                perfil.PerfilDescripcion = controladoraDesc + " - " + accionatributo.AccionDescripcion + " - " + nombreRol;

                db.Perfiles.Add(perfil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionNombre", perfil.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraNombre", perfil.ControladoraID);
            //ViewBag.RolID = new SelectList(db.Roles, "Id", "Name", perfil.RolID);
            ViewBag.RolID = new SelectList(db.Roles, "Id", "Name", perfil.RolID);
            return View(perfil);
        }

        // GET: Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfiles.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionNombre", perfil.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraNombre", perfil.ControladoraID);
            ViewBag.RolID = new SelectList(db.Roles, "Id", "Name", perfil.RolID);
            return View(perfil);
        }

        // POST: Perfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PerfilID,PerfilDescripcion,PerfilInicial,PerfilOwerRequerido,ControladoraID,AccionID,RolID")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccionID = new SelectList(db.Acciones, "AccionID", "AccionNombre", perfil.AccionID);
            ViewBag.ControladoraID = new SelectList(db.Controladoras, "ControladoraID", "ControladoraNombre", perfil.ControladoraID);
            ViewBag.RolID = new SelectList(db.Roles, "Id", "Name", perfil.RolID);
            return View(perfil);
        }

        // GET: Perfil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfiles.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // POST: Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Perfil perfil = db.Perfiles.Find(id);
            db.Perfiles.Remove(perfil);
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
    }
}
