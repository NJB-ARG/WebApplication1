using Microsoft.AspNet.Identity;
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
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.CustomFilters;

namespace WebApplication1.Controllers
{
    public class TareaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tarea
        public ActionResult Index()
        {
            //string currentUserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            //var tareas = db.Tareas.Include(t => t.Tarea_Empleado).Include(t => t.Tarea_Orden).Include(t => t.Tarea_Solicitud);
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //if (UserManager.IsInRole(currentUserId, "Admin"))
            //{
            //    return View(tareas.ToList());
            //}
            //else
            //{
            //    return View(tareas.ToList().Where(x => x.Tarea_AppUser == currentUser));
            //}
            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre");
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID");
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID");
            return View();
        }

        private IEnumerable<Tarea> GetMyTareas()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);            

            if (currentUser != null)
            {
                var tareas = db.Tareas.Include(t => t.Tarea_Empleado).Include(t => t.Tarea_Orden).Include(t => t.Tarea_Solicitud);
                IEnumerable<Tarea> MyToDoes = tareas.ToList();

                int completeCount = 0;
                foreach (Tarea tar in MyToDoes)
                {
                    if (tar.TareaIsDone)
                    {
                        completeCount++;
                    }
                }

                ViewBag.Percent = Math.Round(100f * ((float)completeCount / (float)MyToDoes.Count()));

                //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                //if (UserManager.IsInRole(currentUserId, "Admin"))
                if (IsAdmin(currentUserId))
                {
                    return MyToDoes;
                }
                else
                {
                    return MyToDoes.Where(x => x.Tarea_AppUser == currentUser);
                }
            }
            return null;
        }

        //validar si es usuario admin
        public bool IsAdmin(string userId)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (UserManager.IsInRole(userId, "Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        } 


        //[AuthLog]
        public ActionResult BuildToDoTable()
        {           
                return PartialView("_ToDoTable",GetMyTareas());
        }
        
        // GET: Tarea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            return View(tarea);
        }

        // GET: Tarea/Create
        public ActionResult Create()
        {
            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre");
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID");
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID");
            return View();
        }

        // POST: Tarea/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TareaID,TareaDescripcion,TareaDescripcion2,TareaTipo,TareaModulo,TareaFechaIni,TareaFechaFin,TareaFechaIniReal,TareaFechaFinReal,TareaAprobacion,EmpleadoID,SolicitudID,OrdenID,TareaIsDone,TareaEstado")] Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                tarea.Tarea_AppUser = currentUser;

                db.Tareas.Add(tarea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre", tarea.EmpleadoID);
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", tarea.OrdenID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID", tarea.SolicitudID);
            return View(tarea);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "TareaID,TareaDescripcion,TareaDescripcion2,TareaModulo,TareaFechaIni,TareaFechaFin,EmpleadoID,SolicitudID,OrdenID")] Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                //seteo de valores
                //si es admin tiene que asignar un empleado del cual tomo el usuario sino el mismo usuario
                if (IsAdmin(currentUserId))
                {
                    //el usuario del empleado que seleccionó el ADMIN
                    tarea.Tarea_AppUser = GetUsuarioEmpleado(tarea.EmpleadoID);
                }
                else
                {
                    //el mismo empleado (usuario de logueo y usuario del sistema) que creo la tarea
                    tarea.Tarea_AppUser = currentUser;                    
                    tarea.EmpleadoID = currentUser.ApplicationUser_Persona.PersonaID;
                }
                tarea.TareaTipo = TipoTarea.Manual;                
                tarea.TareaEstado = EstadoTarea.Asignada;
                tarea.TareaIsDone = false;
                //tarea.Tarea_AppUser = currentUser;
                //TareaFechaIniReal,TareaFechaFinReal,SolicitudID,OrdenID
                //tarea.TareaModulo = ModuloTarea.TareaModuloVentas;
                //tarea.TareaFechaIni = DateTime.Now;
                //tarea.TareaFechaFin = DateTime.Now.AddDays(1);
                //tarea.EmpleadoID = currentUser.ApplicationUser_Persona.PersonaID;

                db.Tareas.Add(tarea);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre", tarea.EmpleadoID);
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", tarea.OrdenID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID", tarea.SolicitudID);
            //return View(tarea);
            return PartialView("_ToDoTable", GetMyTareas());
        }

        //recuperar el usuario asociado a un ID de empleado
        public ApplicationUser GetUsuarioEmpleado(int? empleadoID)
        {
            var usuarioEmpleado = db.Empleados.Where(x => x.PersonaID == empleadoID).SingleOrDefault();
            return usuarioEmpleado.Empleado_AppUser; 
        }


        // GET: Tarea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (tarea.Tarea_AppUser != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre", tarea.EmpleadoID);
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", tarea.OrdenID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", tarea.SolicitudID);
            return View(tarea);
        }

        // POST: Tarea/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TareaID,TareaDescripcion,TareaDescripcion2,TareaTipo,TareaModulo,TareaFechaIni,TareaFechaFin,TareaFechaIniReal,TareaFechaFinReal,TareaAprobacion,EmpleadoID,SolicitudID,OrdenID,TareaIsDone,TareaEstado")] Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre", tarea.EmpleadoID);
            ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", tarea.OrdenID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", tarea.SolicitudID);
            return View(tarea);
        }

        //CONTROL DE checkboxes
        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (tarea.TareaFechaIniReal == null)
                {
                    return RedirectToAction("Edit","Tarea", new {id=tarea.TareaID});
                }
                
                //seteo de valores
                tarea.TareaIsDone = value;

                if (tarea.TareaIsDone)
                {
                    tarea.TareaFechaIni = DateTime.Now;
                    tarea.TareaFechaFin = DateTime.Now.AddDays(1);
                }

                db.Entry(tarea).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView("_ToDoTable", GetMyTareas());
            }
            //ViewBag.EmpleadoID = new SelectList(db.Personas, "PersonaID", "PersonaNombre", tarea.EmpleadoID);
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", tarea.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", tarea.SolicitudID);
            //return PartialView("_ToDoTable", GetMyTareas());
        }

        // GET: Tarea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarea tarea = db.Tareas.Find(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            return View(tarea);
        }

        // POST: Tarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarea tarea = db.Tareas.Find(id);
            db.Tareas.Remove(tarea);
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
