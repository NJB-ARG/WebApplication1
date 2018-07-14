using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication1.Models.ViewModels;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    public class Modulo
    {
        public Modulo()
        {
            Modulo_Controladoras = new List<Controladora>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int ModuloID { get; set; }
        public string ModuloDescripcion { get; set; }

        //Nav Prop - lists
        public virtual List<Controladora> Modulo_Controladoras { get; set; }
    }

    public class Controladora
    {
        public Controladora()
        {
            Controladora_Modulo = new Modulo();            
            Controladora_Operaciones = new List<Operacion>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int ControladoraID { get; set; }
        //por atributo
        public double ControladoraNumero { get; set; }
        //por atributo
        public string ControladoraNombre { get; set; }
        //por atributo
        public string ControladoraModulo { get; set; }
        //por reflection
        public string ControladoraDescripcion { get; set; }

        //foreign key
        public int ModuloID { get; set; }
        [ForeignKey("ModuloID")]
        public virtual Modulo Controladora_Modulo { get; set; }

        //Nav Prop - lists        
        public virtual List<Operacion> Controladora_Operaciones { get; set; }
    }

    //Esta dado por una controladora y una acción
    public class Operacion
    {
        public Operacion()
        {
            Operacion_Controladora = new Controladora();
            Operacion_Accion = new Accion();
        }

        [Display(Name = "ID")]
        public int OperacionID { get; set; }

        //la descripcion por reflection
        public string OperacionDescripcion { get; set; }
        public bool OperacionInicial { get; set; }

        //foreign key
        public int ControladoraID { get; set; }
        [ForeignKey("ControladoraID")]
        public virtual Controladora Operacion_Controladora { get; set; }
        //foreign key
        public int AccionID { get; set; }
        [ForeignKey("AccionID")]
        public virtual Accion Operacion_Accion { get; set; }
    }

    //Es una operacion especifica a realizar dado un rol, controladora, acción
    public class Accion
    {
        public Accion()
        {            
            Accion_Operaciones = new List<Operacion>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int AccionID { get; set; }
        public double AccionNumero { get; set; }
        public string AccionNombre { get; set; }
        public string AccionDescripcion { get; set; }
        public bool AccionActivo { get; set; }

        //Nav Prop - lists        
        public virtual List<Operacion> Accion_Operaciones { get; set; }
    }


    //ENTIDAD PARA REGISTRAR LOS PERMISOS DE LOS ROLES
    public class RolOperaciones
    {

        public RolOperaciones()
        {
            RolOperaciones_Rol = new ApplicationRole();
            RolOperaciones_Operacion = new Operacion();
        }

        public int RolOperacionesID { get; set; }             

        public bool RolOperacionesOwerRequired { get; set; }

        public bool RolOperacionesActivo { get; set; }        

        //foreign key
        public string RolID { get; set; }
        [ForeignKey("RolID")]
        public virtual ApplicationRole RolOperaciones_Rol { get; set; }
        //foreign key
        public int OperacionID { get; set; }
        [ForeignKey("OperacionID")]
        public virtual Operacion RolOperaciones_Operacion { get; set; }
    }

    public class ActionLog
    {
        public int ActionLogID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IP { get; set; }
        public DateTime DateTime { get; set; }
    }
    
}