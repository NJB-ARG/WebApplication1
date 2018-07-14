using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Repository;
using WebApplication1.Models.ViewModels;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    //A service layer is an additional layer in an ASP.NET MVC application that mediates communication between a controller and repository layer.
    //The service layer contains business logic. In particular, it contains validation logic.
    public class IdentityService : WebApplication1.Service.IidentityService
    {
        //Notice that the product service is created in the product controller constructor.
        //When the product service is created, the model state dictionary is passed to the service.
        //The product service uses model state to pass validation error messages back to the controller.
        private ModelStateDictionary _modelState;
        private IidentityRepository _repository;

        public IdentityService(ModelStateDictionary modelState, IidentityRepository repository)
        {
            _modelState = modelState;
            _repository = repository;
        }

        //****Metodos de Validación****INI
        //Si aplica, se agregan errores a la propiedad modelstate para mostrarlas en pantalla. IsValid queda igual false
        //protected bool ValidateSolVenta(Solicitud solventaToValidate)
        //{

        //    //validar que el pago se haga sobre una solictud o orden y no ambas. CUANDO HAY ORDEN NO SE PUEDE REGISTRAR UN PAGO SOBRE LA SOLICITUD


        //    if (solventaToValidate.SolicitudDescripcion.Trim().Length == 0)
        //        _modelState.AddModelError("Descripción", "Descripción requerida.");
        //    if (solventaToValidate.SolicitudTipoSolicitante.Trim().Length == 0)
        //        _modelState.AddModelError("Tipo Solicitante", "Tipo Solicitante requerido.");
        //    if (solventaToValidate.SolicitudMontoTotal < 0)
        //        _modelState.AddModelError("MontoTotal", "El monto total no puede ser menor a cero.");
        //    return _modelState.IsValid;
        //}
        //****Metodos de Validación****FIN

        //public IEnumerable<RolOperaciones> ListRolOperaciones()
        //{
        //    return _repository.ListRolOperaciones();
        //}

        //public List<String> EstadosSolVentas()
        //{
        //    return _repository.EstadosSolVentas();
        //}

        //public IQueryable<Solicitud> FilterSolVentas(string searchString, SolicitudStatus solventaestado)
        //{
        //    return _repository.FilterSolVentas(searchString, solventaestado);
        //}

        public async Task<ApplicationRole> BuscarRol(string id)
        {
            return await _repository.BuscarRol(id);
        }

        //public bool CreateSolVenta(Solicitud solventaToCreate)
        //{
        //    // Validation logic
        //    if (!ValidateSolVenta(solventaToCreate))
        //        return false;

        //    // Database logic
        //    try
        //    {
        //        _repository.CreateSolVenta(solventaToCreate);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool EditSolVenta(Solicitud solventaToEdit)
        //{
        //    // Validation logic
        //    if (!ValidateSolVenta(solventaToEdit))
        //        return false;

        //    // Database logic
        //    try
        //    {
        //        _repository.EditSolVenta(solventaToEdit);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool DeleteSolVenta(Solicitud solventaToDelete)
        //{
        //    // Database logic
        //    try
        //    {
        //        _repository.DeleteSolVenta(solventaToDelete);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public void Dispose()
        {
            _repository.Dispose();
        }
    }

    public interface IidentityService
    {
        //bool CreateSolVenta(Solicitud solventaToCreate);
        //IEnumerable<RolOperaciones> ListRolOperaciones();
        //List<String> EstadosSolVentas();
        //IQueryable<Solicitud> FilterSolVentas(string searchString, SolicitudStatus solventaestado);        
        Task<ApplicationRole> BuscarRol(string id);
        //bool EditSolVenta(Solicitud solventaToEdit);
        //bool DeleteSolVenta(Solicitud solventaToDelete);
        void Dispose();
    }
}