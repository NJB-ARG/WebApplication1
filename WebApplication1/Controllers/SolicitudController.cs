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
using System.Data.Entity.Infrastructure;
using WebApplication1.Service;
using WebApplication1.Repository;
using WebApplication1.HtmlHelperExtensions;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Controllers
{
    //This controller uses the repository layer in both its Index() and Create() actions.
    //Notice that this controller does not contain any database logic.
    //Creating a repository layer enables you to maintain a clean separation of concerns.
    //Controllers are responsible for application flow control logic and the repository is responsible for data access logic.

    //[Authorize]
    [HtmlHelperExtensions.MenuItemAttribute(MenuItemNumero = 8.0, MenuItemModulo = "Ventas", MenuItemDescripcion = "Gestión de Solicitudes")]
    [TrazabilidadDatos]    
    public class SolicitudController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ISolVentaService _service;

        public SolicitudController()
        {
            _service = new SolVentaService(this.ModelState, new SolVentaRepository());
        }

        public SolicitudController(ISolVentaService service)
        {
            _service = service;
        }

        //[Authorize(Roles = "Admin,Supervisor")]
        [AuthLog]
        [HtmlHelperExtensions.Action(AccionNumero = 8.1, AccionDescripcion = "Listar Solicitudes", AccionNombre = "Listar", AccionInicial = true)]
        // GET: Solicitud
        public ActionResult Index(string solEst, string solDesc, string solEmp, string solSol, string solSuc, string solNum)
        {

            //Buscar estados posibles para una solicitud de venta y las pasa a la vista
            ViewBag.solEst = new SelectList(_service.EstadosSolVentas());
            PopulateTipoSolicitanteDropDownList();
            //var solicitudes = db.Solicituds
            //                    .Include(c => c.Solicitud_Cliente)
            //                    .Include(c => c.Solicitud_Prospecto)
            //                    .Include(c => c.Solicitud_Empleado)
            //                    .Include(c => c.Solicitud_Paginas)
            //                    .Include(c => c.Solicitud_Ordenes)
            //                    .Include(c => c.Solicitud_LineasSolicitud);
            //return View(db.Solicituds.ToList());
            //return View(solicitudes.ToList());   

            SolicitudStatus estSol = new SolicitudStatus();
            Enum.TryParse(solEst, out estSol);

            //SolicitudStatus MyStatus = new SolicitudStatus();
            //SolicitudStatus MyStatus = (SolicitudStatus)Enum.Parse(typeof(SolicitudStatus), solventaestado, true);
            // Filter para la lista de Solicitudes de venta si hay algun campo de filtro con informacion
            if (!String.IsNullOrEmpty(solDesc) || !string.IsNullOrEmpty(solEst) ||
                !String.IsNullOrEmpty(solEmp) || !string.IsNullOrEmpty(solSol) ||
                !String.IsNullOrEmpty(solSuc) || !string.IsNullOrEmpty(solNum)
                )
            {
                
                var solicitudes_filtradas = _service.FilterSolVentas(solDesc, estSol, solEst, solEmp, solSol, solSuc, solNum);                

                //return View(_service.FilterSolVentas(searchString, solventaestado));
                return View(solicitudes_filtradas);
            }

            return View(_service.ListSolVentas());
        }

        // GET: Solicitud/Details/5
        [HtmlHelperExtensions.Action(AccionNumero = 8.2, AccionDescripcion = "Detalle Solicitud", AccionNombre = "Ver", AccionInicial = true)]
        [AuthLog]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicituds.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }

            //foreach (Solicitud solicitud_filtrada in solicitudes_filtradas)
            //{
            //    foreach (Pago pago in solicitud_filtrada.Solicitud_Pagos)
            //    {
            //        //toma la cuenta nombre del ultimo pago
            //        solicitud_filtrada.CuentaNombre = pago.Pago_Cuenta.CuentaDesc;                                             
            //    }                    
            //}

            //PopulateCuentaDropDownList(solicitud.CuentaID);

            return View(solicitud);
        }

        // GET: Solicitud/Create
        [HtmlHelperExtensions.Action(AccionNumero = 8.3, AccionDescripcion = "Nueva Solicitud", AccionNombre = "Crear", AccionInicial = true)]
        [AuthLog]
        public ActionResult Create()
        {
            //PopulateCuentaDropDownList();
            PopulateClienteDropDownList();
            PopulateProspectoDropDownList();
            PopulateEmpleadoDropDownList();
            PopulateTipoSolicitanteDropDownList();
            PopulateSucursalDropDownList();
            //PopulateEstadosDropDownList(Functions.GetEnumDescription(SolicitudStatus.En_Borrador));

            return View();
        }

        // POST: Solicitud/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SolicitudID,SolicitudNum,SolicitudDescripcion,EmpleadoID,ProspectoID,ClienteID,SolicitudTipoSolicitante,SolicitudFecCreacion,SolicitudFecVencimiento,SucursalID,SolicitudEstado,SolicitudMontoTotal")] Solicitud solicitud)
        {

            //solicitud.SolicitudOwnerID = HttpContext.User.Identity.Name;
            try
            {
                if (ModelState.IsValid)
                {                    
                    //******
                    //LLAMADA PATRONES-INI
                    //------------------------------------------------------------------------------------------------------------------------------------------
                    //--CREACIÓN DEL OBJETO COMPLEJO "SOLICITUD" PASO 1--
                    //Patrón Builder: CREA un documento con sus respectivas páginas de datos vacías, las cuales se completan a medida que se avanza con el desarrollo del documento en cuestión  
                    //------------------------------------------------------------------------------------------------------------------------------------------

                    //The 'Builder' abstract class. La clase abstracta para asignar la clase concreta que quiero crear. En este caso: SOLICITUD
                    DocumentoBuilder solicitudBuilder;

                    //The 'Director' class. Es la clase que usa el objeto complejo concreto para ejecutar el método "construir" que tiene los pasos de creación de cada parte de un documento
                    DirectorDocumento directorSolicitud = new DirectorDocumento();

                    // Construyo el documento complejo que necesite vacío
                    solicitudBuilder = new SolicitudBuilder();
                    directorSolicitud.Construir(solicitudBuilder);

                    //Cargo el list de paginas del documento en cración. VIEWMODEL a renderizar en la vista
                    //ConstructorDocumento.Documento.partes

                    solicitud.Solicitud_Paginas = solicitudBuilder.Documento.paginas.Values.ToList();

                    Documento doc = new Documento();
                    doc = solicitudBuilder.Documento;
                    //LLAMADA PATRONES-FIN
                    //******


                    solicitud.SolicitudOwnerID = HttpContext.User.Identity.Name;
                    if (solicitud.SolicitudTipoSolicitante == "1")
                    {
                        solicitud.ClienteID = null;
                        solicitud.Solicitud_Cliente = null;
                    }
                    else if (solicitud.SolicitudTipoSolicitante == "2")
                    {
                        solicitud.ProspectoID = null;
                        solicitud.Solicitud_Prospecto = null;
                    }

                    if (ModelState.IsValid)
                    {

                        //if (!_service.CreateSolVenta(solicitud))
                        if (_service.CreateSolVentaID(solicitud) == 0)
                        return View();

                        //return RedirectToAction("Index");
                        return RedirectToAction("Create", "LineaSolicitud", new { solicitudID = solicitud.SolicitudID });

                    //db.Solicituds.Add(solicitud);
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                    }                    
                    //    ModelState.AddModelError("", "Debe cargar al menos un Producto en las Líneas de Detalle de la Solicitud antes de Guardar");             
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            //PopulateCuentaDropDownList(solicitud.CuentaID);

            //if (solicitud.SolicitudTipoSolicitante == "Cliente")
            //{
                PopulateClienteDropDownList(solicitud.ClienteID);
            //}
            //else if (solicitud.SolicitudTipoSolicitante == "Cliente")
            //{
                PopulateProspectoDropDownList(solicitud.ProspectoID);
            //}

            PopulateEmpleadoDropDownList(solicitud.EmpleadoID);

            PopulateSucursalDropDownList(solicitud.SucursalID);

            PopulateEstadosDropDownList(solicitud.SolicitudEstado);

            PopulateTipoSolicitanteDropDownList(solicitud.SolicitudTipoSolicitante);

            return View(solicitud);
        }

        // GET: Solicitud/Edit/5        
        [HtmlHelperExtensions.Action(AccionNumero = 8.4, AccionDescripcion = "Editar Solicitud", AccionNombre = "Modificar", AccionInicial = true)]
        [AuthLog]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Solicitud solicitud = db.Solicituds.Find(id);
            //This code adds eager loading for the associated Cuenta entity. You can't perform eager loading with the Find method, so the Where and Single methods are used instead to select the solicitud.
            //Solicitud solicitud = db.Solicituds
            //    .Include(i => i.Solicitud_Pagos)
            //    .Where(i => i.SolicitudID == id)
            //    .Single();

            var solicitud = db.Solicituds
                .Include(c => c.Solicitud_Cliente)
                .Include(c => c.Solicitud_Prospecto)
                .Include(c => c.Solicitud_Empleado)
                .Include(c => c.Solicitud_Paginas)
                .Include(c => c.Solicitud_Ordenes)
                .Include(c => c.Solicitud_Sucursal)
                .Include(c => c.Solicitud_LineasSolicitud)
                .Include(c => c.Solicitud_Pagos.Select(p => p.Pago_Cuenta))
                .Where(i => i.SolicitudID == id)
                .Single();

            PopulateAssignedPaginasData(solicitud);

            double monto = new double();
            monto = 0;
            foreach (LineaSolicitud linea in solicitud.Solicitud_LineasSolicitud)
            {
                monto = monto + linea.LineaSolicitudMonto;
                solicitud.SolicitudMontoTotal = monto;
            }

            if (solicitud == null)
            {
                return HttpNotFound();
            }

            //PopulateCuentaDropDownList(solicitud.CuentaID);

            PopulateClienteDropDownList(solicitud.ClienteID);

            PopulateProspectoDropDownList(solicitud.ProspectoID);

            PopulateEmpleadoDropDownList(solicitud.EmpleadoID);

            PopulateSucursalDropDownList(solicitud.SucursalID);

            PopulateEstadosDropDownList(solicitud.SolicitudEstado);

            PopulateTipoSolicitanteDropDownList(solicitud.SolicitudTipoSolicitante);

            return View(solicitud);
        }

        // POST: Solicitud/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SolicitudID,SolicitudNum,SolicitudDescripcion,EmpleadoID,ProspectoID,ClienteID,SolicitudTipoSolicitante,SolicitudFecCreacion,SolicitudFecVencimiento,SolicitudSucursal,SolicitudEstado,CuentaID,SolicitudMontoTotal")] Solicitud solicitud)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(solicitud).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(solicitud);
        //}

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedPaginas)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            
            //var solicitudToUpdate = db.Solicituds.Find(id);
            var solicitudToUpdate = db.Solicituds
                .Include(c => c.Solicitud_Cliente)
                .Include(c => c.Solicitud_Prospecto)
                .Include(c => c.Solicitud_Empleado)
                .Include(c => c.Solicitud_Paginas)
                .Include(c => c.Solicitud_Ordenes)
                .Include(c => c.Solicitud_Sucursal)
                .Include(c => c.Solicitud_LineasSolicitud)
                .Include(c => c.Solicitud_Pagos.Select(p => p.Pago_Cuenta))
                .Where(i => i.SolicitudID == id)
                .Single();

            if (_service.RecuperarLineas(solicitudToUpdate.SolicitudID).Count() == 0)
            {
                solicitudToUpdate.SolicitudEstado = SolicitudStatus.En_Borrador;
            }

            double monto = new double();
            monto = 0;
            foreach (LineaSolicitud linea in solicitudToUpdate.Solicitud_LineasSolicitud)
            {
                monto = monto + linea.LineaSolicitudMonto;
                solicitudToUpdate.SolicitudMontoTotal = monto;
            }

            if (TryUpdateModel(solicitudToUpdate, "",
               new string[] { "SolicitudID", "SolicitudNum", "SolicitudDescripcion", "EmpleadoID", "ProspectoID", "ClienteID", "SolicitudTipoSolicitante", "SolicitudFecCreacion", "SolicitudFecVencimiento", "SucursalID", "SolicitudEstado", "SolicitudMontoTotal"}))                                                                                                                            
            {
                try
                {
                    //var cuentaDesc = db.Cuentas.Where(i => i.CuentaID == solicitudToUpdate.CuentaID).SingleOrDefault().CuentaDesc;

                    //if (String.IsNullOrWhiteSpace(cuentaDesc))
                    //{
                    //    solicitudToUpdate.CuentaID = null;
                    //}

                    UpdateSolicitudPaginas(selectedPaginas, solicitudToUpdate);

                    if (solicitudToUpdate.SolicitudTipoSolicitante == "1")
                    {
                        solicitudToUpdate.ClienteID = null;
                        solicitudToUpdate.Solicitud_Cliente = null;
                    }
                    else if (solicitudToUpdate.SolicitudTipoSolicitante == "2")
                    {
                        solicitudToUpdate.ProspectoID = null;
                        solicitudToUpdate.Solicitud_Prospecto = null;
                    }

                    //***CREACION DE LA ORDEN
                    if (solicitudToUpdate.SolicitudEstado == SolicitudStatus.Aprobada)
                    {
                        if (solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaValidada == false && r.PaginaTipo == "C").Count() > 0)
                        {
                            //la vuelvo a En Aprobación
                            solicitudToUpdate.SolicitudEstado = SolicitudStatus.En_Aprobacion;
                            db.SaveChanges();

                            var vr = new ViewResult();
                            vr.ViewName = "CustomInfo";

                            ViewDataDictionary dict = new ViewDataDictionary();
                            dict.Add("Message", "No se pudo Aprobar la Solicitud. Todos los documentos de la Solicitud deben estar aprobados antes de crear una Orden");

                            vr.ViewData = dict;

                            var result = vr;
                            return result ;                            
                        }
                        else
                        {
                            db.SaveChanges();

                            return RedirectToAction("Create", "Orden", new { idSolicitud = solicitudToUpdate.SolicitudID });
                        }
                    } 
                    //***                   

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedPaginasData(solicitudToUpdate);
            //PopulateCuentaDropDownList(solicitudToUpdate.CuentaID);
            PopulateClienteDropDownList(solicitudToUpdate.ClienteID);
            PopulateProspectoDropDownList(solicitudToUpdate.ProspectoID);
            PopulateEmpleadoDropDownList(solicitudToUpdate.EmpleadoID);
            PopulateSucursalDropDownList(solicitudToUpdate.SucursalID);
            PopulateEstadosDropDownList(solicitudToUpdate.SolicitudEstado);
            PopulateTipoSolicitanteDropDownList(solicitudToUpdate.SolicitudTipoSolicitante);

            return View(solicitudToUpdate);
        }


        //Populate DropDown Methods
        //*con acceso a DB
        private void PopulateCuentaDropDownList(object selectedCuenta = null)
        {
            var cuentasQuery = from d in db.Cuentas
                                   orderby d.CuentaDesc
                                   select d;
            ViewBag.CuentaID = new SelectList(cuentasQuery, "CuentaID", "CuentaDesc", selectedCuenta);
        }

        private void PopulateClienteDropDownList(object selectedCliente = null)
        {
            var clientesQuery = from d in db.Personas.OfType<Cliente>()
                                           orderby d.PersonaApellido
                                   select d;
            ViewBag.ClienteID = new SelectList(clientesQuery, "PersonaID", "FullName", selectedCliente);
        }

        private void PopulateProspectoDropDownList(object selectedProspecto = null)
        {
            var prospectosQuery = from d in db.Personas.OfType<Prospecto>()
                                orderby d.PersonaApellido
                                select d;
            ViewBag.ProspectoID = new SelectList(prospectosQuery, "PersonaID", "FullName", selectedProspecto);
        }

        private void PopulateEmpleadoDropDownList(object selectedEmpleado = null)
        {
            var EmpleadosQuery = from d in db.Personas.OfType<Empleado>()
                                   orderby d.PersonaApellido
                                   select d;
            ViewBag.EmpleadoID = new SelectList(EmpleadosQuery, "PersonaID", "FullName", selectedEmpleado);
        }

        private void PopulateSucursalDropDownList(object selectedSucursal = null)
        {
            var SucursalQuery = from d in db.Sucursales
                                 orderby d.SucursalNombre
                                 select d;
            ViewBag.SucursalID = new SelectList(SucursalQuery, "SucursalID", "SucursalNombre", selectedSucursal);
        }

        private void PopulateEstadosDropDownList(object selectedEstado = null)
        {
            ViewBag.EstadosSolicitud = new SelectList(_service.EstadosSolVentas(), selectedEstado);
        }

        private void PopulateTipoSolicitanteDropDownList(object SelectedTipo = null)
        {
            //var TiposSolicitantesQuery = (from d in db.Solicituds
            //                              orderby d.SolicitudID descending
            //                              select d).GroupBy(g => g.SolicitudTipoSolicitante).Select(x => x.FirstOrDefault());
            
            var TiposSolicitantes = new Custom_Dropdownlist();
            TiposSolicitantes.dropdownlist = new[]
            {
                new SelectListItem { Value = "1", Text = "Prospecto"},
                new SelectListItem { Value = "2", Text = "Cliente" },                
            };

            //ViewBag.TipoSolicitante = new SelectList(TiposSolicitantesQuery, "SolicitudID", "SolicitudTipoSolicitante");
            ViewBag.SolicitudTipoSolicitante = new SelectList(TiposSolicitantes.dropdownlist, "Value", "Text", SelectedTipo);
        }

        private void PopulateAssignedPaginasData(Solicitud solicitudToUpdate)
        {
            var allPaginasSolCab = db.Paginas.Where(r => r.Pagina_Solicitud.SolicitudID == solicitudToUpdate.SolicitudID && r.PaginaTipo == "C");
            var solicitudPaginasVal = new HashSet<int>(solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaValidada == true && r.PaginaTipo == "C").Select(c => c.PaginaID));
            var viewModel = new List<PaginaValidadaViewModel>();
            foreach (var pagina in allPaginasSolCab)
            {
                viewModel.Add(new PaginaValidadaViewModel
                {
                    PaginaID = pagina.PaginaID,
                    PaginaNumero = pagina.PaginaNumero,
                    Nombre = pagina.PaginaNombre,
                    Validada = solicitudPaginasVal.Contains(pagina.PaginaID)
                });
            }
            //paginas de cabecera para vlidadción de estado
            ViewBag.Paginas = viewModel;

            var allPaginasSol = db.Paginas.Where(r => r.Pagina_Solicitud.SolicitudID == solicitudToUpdate.SolicitudID);
            var viewModelPaginas = new SolicitudPaginasViewModel();
            foreach (var pagina in allPaginasSol)
            {                                
                if (pagina.PaginaNombre == "Cuenta Depósito")
                {
                    viewModelPaginas.CuentaDeposito = db.CuentaDeposito.Where(r => r.PaginaID == pagina.PaginaID).Single();
                }
                else if (pagina.PaginaNombre == "Oferta Mercado")
                {
                    viewModelPaginas.OfertasMercado = db.OfertasMercado.Where(r => r.PaginaID == pagina.PaginaID).Single();
                }
                else if (pagina.PaginaNombre == "Propuesta Comercial")
                {
                    viewModelPaginas.PropuestaComercial = db.PropuestaComercial.Where(r => r.PaginaID == pagina.PaginaID).Single();
                }
                else if (pagina.PaginaNombre == "Datos de Unidad")
                {
                    viewModelPaginas.DatosUnidad = db.DatosUnidad.Where(r => r.PaginaID == pagina.PaginaID).Single();
                }
                else if (pagina.PaginaNombre == "Datos de Contacto")
                {
                    viewModelPaginas.DatosContacto = db.DatosContacto.Where(r => r.PaginaID == pagina.PaginaID).Single();
                }
                else if (pagina.PaginaNombre == "Documentos Adjuntos")
                {
                    var adjunto = db.DocAdjunto.Where(r => r.PaginaID == pagina.PaginaID).Single();
                    viewModelPaginas.DocAdjuntos.Add(adjunto);
                }
                else if (pagina.PaginaNombre == "Pagos Realizados")
                {
                    var pago = db.PagosRealizados.Where(r => r.PaginaID == pagina.PaginaID).Single();
                    viewModelPaginas.PagosRealizados.Add(pago); 
                }
                else if (pagina.PaginaNombre == "Comentarios")
                {
                    var comentario = db.Comentario.Where(r => r.PaginaID == pagina.PaginaID).Single();
                    viewModelPaginas.Comentarios.Add(comentario);
                }                            
            }
            ViewBag.PaginasSol = viewModelPaginas;
        }

        private void UpdateSolicitudPaginas(string[] selectedPaginas, Solicitud solicitudToUpdate)
        {
            if (selectedPaginas == null)
            {
                foreach (Pagina pag in solicitudToUpdate.Solicitud_Paginas)
                {
                    //solicitudToUpdate.Solicitud_Paginas = new List<Pagina>();
                    solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaID == pag.PaginaID).SingleOrDefault().PaginaValidada = false;
                }                
                return;
            }

            var selectedPaginasHS = new HashSet<string>(selectedPaginas);
            //var solicitudPaginasVal = new HashSet<int>
            //    (solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaValidada == true).Select(c => c.PaginaID));
            foreach (var pagina in db.Paginas.Where(r => r.Pagina_Solicitud.SolicitudID == solicitudToUpdate.SolicitudID))
            {
                //la pagina de la solicitud que estoy leyendo es una de las seleccionadas?
                if (selectedPaginasHS.Contains(pagina.PaginaID.ToString()))
                {
                    solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaID == pagina.PaginaID).SingleOrDefault().PaginaValidada = true;
                    //la pagina de la solicitud que estoy leyendo ya está validada?
                    //if (!solicitudPaginasVal.Contains(pagina.PaginaID))
                    //{
                    //    //solicitudToUpdate.Solicitud_Paginas.Add(pagina);                          
                    //}
                }
                else
                {
                    solicitudToUpdate.Solicitud_Paginas.Where(r => r.PaginaID == pagina.PaginaID).SingleOrDefault().PaginaValidada = false;
                    //if (solicitudPaginasVal.Contains(pagina.PaginaID))
                    //{
                    //    solicitudToUpdate.Solicitud_Paginas.Remove(pagina);
                    //}
                }
            }
        }

        // GET: Solicitud/Delete/5       
        [HtmlHelperExtensions.Action(AccionNumero = 8.5, AccionDescripcion = "Eliminar Solicitud", AccionNombre = "Borrar", AccionInicial = false)]
        [AuthLog]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicituds.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        // POST: Solicitud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitud solicitud = db.Solicituds.Find(id);
            db.Solicituds.Remove(solicitud);
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
