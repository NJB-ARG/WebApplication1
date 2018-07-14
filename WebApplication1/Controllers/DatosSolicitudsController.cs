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
    public class DatosSolicitudsController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();

        // GET: DatosSolicitudPaginas
        public ActionResult Index()
        {
            //string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
            //    + "FROM Person "
            //    + "WHERE Discriminator = 'Student' "
            //    + "GROUP BY EnrollmentDate";
            //IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

            //return View(data.ToList());

            return View(db.DatosSolicitudPaginas.ToList());
        }

        // GET: DatosSolicitudPaginas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitudPagina datosSolicitud = db.DatosSolicitudPaginas.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // GET: DatosSolicitudPaginas/Create
        public ActionResult Create()
        {
            //Es el objeto complejo que estoy creando que me sirve de auxiliar para ir creando la VENTA paso a paso
            DocumentoBuilder solicitudBuilder;

            //Es la clase que usando el objeto complejo concreto, ejecuta el método con los pasos creación
            DirectorDocumento directorSolicitud = new DirectorDocumento();

            // Construyo el documento complejo que necesite (SOLICITUD con sus paginas u ORDEN con sus paginas) vacío
            solicitudBuilder = new SolicitudBuilder();
            directorSolicitud.Construir(solicitudBuilder);

            //Cargo el VIEWMODEL a renderizar en la vista
            //ConstructorDocumento.Documento.partes
            return View(solicitudBuilder.Documento.paginas);
            //return View();
        }

        // POST: DatosSolicitudPaginas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DatosSolicitudID,DatosSolicitudNombre,PaginaID,PaginaNombre")] DatosSolicitudPagina datosSolicitud)
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
                DocumentoBuilder solicitudBuilder;

                //Es la clase que usando el objeto complejo concreto, ejecuta el método con los pasos creación
                DirectorDocumento directorSolicitud = new DirectorDocumento();

                // Construyo el documento complejo que necesite (SOLICITUD con sus paginas u ORDEN con sus paginas) vacío
                solicitudBuilder = new SolicitudBuilder(datosSolicitud);
                directorSolicitud.Construir(solicitudBuilder);

                //Cargo el VIEWMODEL a renderizar en la vista
                //ConstructorDocumento.Documento.partes

                //solicitudBuilder.Documento.paginas
                
                //creo una SOLICITUD nueva
                //Solicitud nuevaSolicitud = new Solicitud();




                //Creo una VENTA nueva
                //Venta nuevaVenta = new Venta();
                //nuevaVenta.VentaID = documentoBuilder.IdDocumento;
                //nuevaVenta.VentaTitulo = "Venta para documento numero: " + solicitudBuilder.IdDocumento;

               

                //creo una ORDEN nueva (NO DESARROLLADO)
                //Orden nuevaOrden = new Orden();

                //cargo la solicitud nueva
                //PATRONES...
                // Recorre el Dictionary para cada par de valores dentro del objeto complejo creado
                //foreach (KeyValuePair<string, Pagina> parte_par in documentoBuilder.Documento.partes)
                //{

                //    if (parte_par.Key == "cabecera")
                //    {
                //        //paginas de cabecera
                //        foreach (Pagina paginaCabecera in parte_par.Value.Paginas)
                //        {
                //            //Cargo la pagina datos solicitud del objeto complejo SolicitudBuilder
                //            Type type = paginaCabecera.GetType();
                            
                //            //En cada caso debería desarrollar la funcionalidad de negocio (PATRON TEMPLATE METOD?)
                //            if (type.Equals(typeof(DatosSolicitudPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new DatosSolicitudPagina { PaginaID = paginaCabecera.PaginaID, PaginaNombre = paginaCabecera.PaginaNombre, DatosSolicitudTitulo = "Pagina Datos Solicitud" });
                //            }

                //            if (type.Equals(typeof(CuentaBancariaDepositoPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new CuentaBancariaDepositoPagina { PaginaID = paginaCabecera.PaginaID, PaginaNombre = paginaCabecera.PaginaNombre });
                //            }

                //            if (type.Equals(typeof(PagosRealizadosPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new PagosRealizadosPagina { PaginaID = paginaCabecera.PaginaID, PaginaNombre = paginaCabecera.PaginaNombre });
                //            }

                //            if (type.Equals(typeof(DatosUnidadPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new DatosUnidadPagina { PaginaID = paginaCabecera.PaginaID, PaginaNombre = paginaCabecera.PaginaNombre });
                //            }

                //        }
                //    }
                //    else if (parte_par.Key == "detalle")
                //    {
                //        //paginas de detalle
                //        foreach (Pagina paginaDetalle in parte_par.Value.Paginas)
                //        {
                //            //Cargo la pagina datos solicitud del objeto complejo SolicitudBuilder
                //            Type type = paginaDetalle.GetType();

                //            //En cada caso debería desarrollar la funcionalidad de negocio (PATRON TEMPLATE METOD?)
                //            if (type.Equals(typeof(HojaProspectoPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new HojaProspectoPagina { PaginaID = paginaDetalle.PaginaID, PaginaNombre = paginaDetalle.PaginaNombre });
                //            }

                //            if (type.Equals(typeof(PropuestaComercialPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new PropuestaComercialPagina { PaginaID = paginaDetalle.PaginaID, PaginaNombre = paginaDetalle.PaginaNombre });
                //            }

                //            if (type.Equals(typeof(LegajoPersonaPagina)))
                //            {
                //                nuevaSolicitud.SolicitudPaginas.Add(new LegajoPersonaPagina { PaginaID = paginaDetalle.PaginaID, PaginaNombre = paginaDetalle.PaginaNombre });
                //            }
                //        }
                //    }

                //}


                //Asigno la Orden a la venta: ID
                //nuevaVenta.OrdenID = documentoBuilder.IdDocumento;
                //nuevaVenta.VentaOrden = nuevaOrden;
                //ALTA DE VENTA EN DB
                //db.Ventas.Add(nuevaVenta);
                //db.SaveChanges();
                //******************************************************************************************************************************************

                db.DatosSolicitudPaginas.Add(datosSolicitud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

             return View(datosSolicitud);
        }

        // GET: DatosSolicitudPaginas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitudPagina datosSolicitud = db.DatosSolicitudPaginas.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // POST: DatosSolicitudPaginas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DatosSolicitudID,DatosSolicitudNombre,PaginaID,PaginaNombre")] DatosSolicitudPagina datosSolicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datosSolicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(datosSolicitud);
        }

        // GET: DatosSolicitudPaginas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatosSolicitudPagina datosSolicitud = db.DatosSolicitudPaginas.Find(id);
            if (datosSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(datosSolicitud);
        }

        // POST: DatosSolicitudPaginas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DatosSolicitudPagina datosSolicitud = db.DatosSolicitudPaginas.Find(id);
            db.DatosSolicitudPaginas.Remove(datosSolicitud);
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
