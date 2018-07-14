using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DatosSolicitudController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();

        // GET: DatosSolicituds
        public ActionResult Index()
        {
            return View(db.DatosSolicituds.ToList());
        }

        // GET: DatosSolicituds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitud datosSolicitud = db.DatosSolicituds.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // GET: DatosSolicituds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatosSolicituds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DatosSolicitudID,DatosSolicitudNombre")] DatosSolicitud datosSolicitud)
        {
            if (ModelState.IsValid)
            {

                //******************************************************************************************************************************************
                //ESTOS 2 PATRONES ME PERMITEN CREAR OBJETOS COMPLEJOS PARA LABURARLOS EN TIEMPO DE EJECUCION Y PEGAR SOBRE LA BD
                //patron template o strategy para crear una VENTA
                //******************************************************************************************************************************************

                //------------------------------------------------------------------------------------------------------------------------------------------
                //--CREACIÓN DEL OBJETO COMPLEJO "SOLICITUD" PASO 1--
                //Patrón Builder: CREA la PARTE1 del objeto complejo y este objeto queda disponible para crear sus siguientes partes 
                //------------------------------------------------------------------------------------------------------------------------------------------

                //Es el objeto complejo que estoy creando que me sirve de auxiliar para ir creando la VENTA paso a paso
                DocumentoBuilder documentoBuilder;

                //Es la clase que usando el objeto complejo concreto, ejecuta el método con los pasos creación
                DirectorDocumento directorDocumento = new DirectorDocumento();

                // Construyo el documento complejo que necesite (SOLICITUD con sus paginas u ORDEN con sus paginas) vacío
                documentoBuilder = new SolicitudBuilder(datosSolicitud);
                directorDocumento.Construir(documentoBuilder);

                //Objeto que devuelve el patron (PRODUCTO): Un documento puede ser una SOLICITUD o una ORDEN
                //documentoBuilder.Documento;

                //------------------------------------------------------------------------------------------------------------------------------------------
                //--VENTA CONCRETADA--
                //Patrón Builder: CREA la PARTE1 del objeto complejo y este objeto queda disponible para crear sus siguientes partes 
                //------------------------------------------------------------------------------------------------------------------------------------------


                //Copio el documento (producto) creado:  

                // Recorre el Dictionary para cada par de valores dentro del objeto complejo creado
                foreach (KeyValuePair<string, PartesDocumento> parte_par in documentoBuilder.Documento._partes)
                {

                    if (parte_par.Key == "cabecera")
                    {

                        //MyClass result = list.Find(x => x.Id == "xy");
                        //MyClass result = list.Find(x => x.GetId() == "xy");

                        Pagina pagina_aux = new DatosSolicitud();

                        foreach (Pagina pagina in parte_par.Value.Paginas)
                        {
                            //Cargo la pagina datos solicitud del objeto complejo SolicitudBuilder
                            Type type = pagina.GetType();
                            if (type.Equals(typeof(DatosSolicitud)))
                                pagina_aux = pagina;

                        }

                        var index = parte_par.Value.Paginas.IndexOf(pagina_aux);

                        if (index != -1)
                            parte_par.Value.Paginas[index] = datosSolicitud;

                    }          

                }

                // Cargo los datos del formulario "datosSolicitud" recibido al documento complejo creado en el paso anterior
                //documentoBuilder.Documento["cabecera"].Paginas.Find(x => x.PaginaID == 1) = datosSolicitud;

                // Copio el documento complejo recién creado con las ultimas modificaciones, a la parte que corresponda de la nueva VENTA
                //Venta ventaNueva = new Ventas();
                //ventaNueva.VentaSolicitud = documentoBuilder.Documento
                // Grabo la nueva venta
                //db.Ventas.Add(Venta);

                db.DatosSolicituds.Add(datosSolicitud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(datosSolicitud);
        }

        // GET: DatosSolicituds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitud datosSolicitud = db.DatosSolicituds.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // POST: DatosSolicituds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DatosSolicitudID,DatosSolicitudNombre")] DatosSolicitud datosSolicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datosSolicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(datosSolicitud);
        }

        // GET: DatosSolicituds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitud datosSolicitud = db.DatosSolicituds.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // POST: DatosSolicituds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DatosSolicitud datosSolicitud = db.DatosSolicituds.Find(id);
            db.DatosSolicituds.Remove(datosSolicitud);
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
