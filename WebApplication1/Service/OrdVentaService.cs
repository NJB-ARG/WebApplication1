using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Repository;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Service
{
    //A service layer is an additional layer in an ASP.NET MVC application that mediates communication between a controller and repository layer.
    //The service layer contains business logic. In particular, it contains validation logic.
    public class OrdVentaService : WebApplication1.Service.IOrdVentaService
    {
        //Notice that the product service is created in the product controller constructor.
        //When the product service is created, the model state dictionary is passed to the service.
        //The product service uses model state to pass validation error messages back to the controller.
        private ModelStateDictionary _modelState;
        private IOrdVentaRepository _repository;

        public OrdVentaService(ModelStateDictionary modelState, IOrdVentaRepository repository)
        {
            _modelState = modelState;
            _repository = repository;
        }

        //****Metodos de Validación****INI
        //Si aplica, se agregan errores a la propiedad modelstate para mostrarlas en pantalla. IsValid queda igual false
        protected bool ValidateSolVenta(Orden ordventaToValidate)
        {

            //validar que la solicitud a crear tenga al menos una linea de detalle

            //validar que el pago se haga sobre una solictud o orden y no ambas. CUANDO HAY ORDEN NO SE PUEDE REGISTRAR UN PAGO SOBRE LA SOLICITUD

            //validar: documentos adjuntos cargado true (sería el estado)

            if (ordventaToValidate.Orden_Solicitud.SolicitudFecCreacion > ordventaToValidate.Orden_Solicitud.SolicitudFecVencimiento)
                _modelState.AddModelError("Fecha Creación", "La Fecha de Vencimiento debe ser mayor a la Fecha de Creación");

            if (ordventaToValidate.Orden_Solicitud.SolicitudDescripcion.Trim().Length == 0)
                _modelState.AddModelError("Descripción", "Descripción requerida.");
            if (ordventaToValidate.Orden_Solicitud.SolicitudTipoSolicitante.Trim().Length == 0)
                _modelState.AddModelError("Tipo Solicitante", "Tipo Solicitante requerido.");
            if (ordventaToValidate.Orden_Solicitud.SolicitudMontoTotal < 0)
                _modelState.AddModelError("MontoTotal", "El monto total no puede ser menor a cero.");
            return _modelState.IsValid;
        }
        //****Metodos de Validación****FIN

        public IEnumerable<Solicitud> ListSolVentas()
        {
            return _repository.ListSolVentas();
        }

        public List<String> EstadosOrdVentas()
        {
            return _repository.EstadosOrdVentas();
        }

        public IQueryable<Solicitud> FilterSolVentas(string solDesc, SolicitudStatus estSol, string solEst, string solEmp, string solSol, string solSuc, string solNum)
        {
            return _repository.FilterSolVentas(solDesc, estSol, solEst, solEmp, solSol, solSuc, solNum);
        }

        public Solicitud BuscarSolVenta(int? id)
        {
            return _repository.BuscarSolVenta(id);
        }

        public bool CreateOrdVenta(Orden ordventaToCreate)
        {
            // Validation logic
            if (!ValidateSolVenta(ordventaToCreate))
                return false;

            // Database logic
            try
            {
                _repository.CreateOrdVenta(ordventaToCreate);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool EditOrdVenta(Orden ordventaToEdit)
        {
            // Validation logic
            if (!ValidateSolVenta(ordventaToEdit))
                return false;

            // Database logic
            try
            {
                _repository.EditOrdVenta(ordventaToEdit);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteSolVenta(Solicitud solventaToDelete)
        {
            // Database logic
            try
            {
                _repository.DeleteSolVenta(solventaToDelete);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        //
        public IQueryable<LineaSolicitud> RecuperarLineas(int id_solventa)
        {
            return _repository.RecuperarLineas(id_solventa);
        }

        public int CreateSolVentaID(Solicitud solventaToCreate)
        {
            return _repository.CreateSolVentaID(solventaToCreate);
        }
    }

    public interface IOrdVentaService
    {
        bool CreateOrdVenta(Orden ordventaToCreate);
        IEnumerable<Solicitud> ListSolVentas();
        List<String> EstadosOrdVentas();
        IQueryable<Solicitud> FilterSolVentas(string solDesc, SolicitudStatus estSol, string solEst, string solEmp, string solSol, string solSuc, string solNum);
        Solicitud BuscarSolVenta(int? id);
        bool EditOrdVenta(Orden ordventaToEdit);
        bool DeleteSolVenta(Solicitud solventaToDelete);
        void Dispose();

        //
        IQueryable<LineaSolicitud> RecuperarLineas(int id_solventa);
        int CreateSolVentaID(Solicitud solventaToCreate);
    }
}