using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Descripción")]
        public string ModuloDescripcion { get; set; }

        //Nav Prop - lists
        public virtual List<Controladora> Modulo_Controladoras { get; set; }
    }

    public class Controladora
    {
        public Controladora()
        {
            //Controladora_Modulo = new Modulo();
            Controladora_Perfiles = new List<Perfil>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int ControladoraID { get; set; }
        //por atributo
        [Display(Name = "Número")]
        public double ControladoraNumero { get; set; }
        //por atributo
        [Display(Name = "Controladora")]
        public string ControladoraNombre { get; set; }
        //por atributo
        [Display(Name = "Nombre")]
        public string ControladoraNombreFull { get; set; }
        //por atributo
        [Display(Name = "Módulo Controladora")]
        public string ControladoraModulo { get; set; }
        //por reflection
        [Display(Name = "Descripción")]
        public string ControladoraDescripcion { get; set; }

        //foreign key        
        public int ModuloID { get; set; }
        [ForeignKey("ModuloID")]
        public virtual Modulo Controladora_Modulo { get; set; }

        //Nav Prop - lists        
        public virtual List<Perfil> Controladora_Perfiles { get; set; }

        //Nav Prop - lists        
        //public virtual List<Accion> Controladora_Acciones { get; set; }
    }

    //Es una Perfil especifica a realizar dado un rol, controladora, acción
    public class Accion
    {
        public Accion()
        {
            Accion_Perfiles = new List<Perfil>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        public int AccionID { get; set; }
        [Display(Name = "Número")]
        public double AccionNumero { get; set; }
        [Display(Name = "Accion")]
        public string AccionNombre { get; set; }
        [Display(Name = "Descripción")]
        public string AccionDescripcion { get; set; }
        [Display(Name = "Activo")]
        public bool AccionActivo { get; set; }

        //Nav Prop - lists        
        public virtual List<Perfil> Accion_Perfiles { get; set; }

        //Nav Prop - lists        
        //public virtual List<Controladora> Accion_Controladoras { get; set; }
    }


    //Esta dado por una controladora, una acción, un rol
    public class Perfil
    {
        //public Perfil()
        //{
        //    Perfil_Controladora = new Controladora();
        //    Perfil_Accion = new Accion();
        //    Perfil_Rol = new ApplicationRole();
        //}

        [Display(Name = "ID")]
        public int PerfilID { get; set; }
        
        [Display(Name = "Descripción")]
        public string PerfilDescripcion { get; set; }
        [Display(Name = "Inicial por Defecto")]
        public bool PerfilInicial { get; set; }
        [Display(Name = "Owner Requerido")]
        public bool PerfilOwerRequerido { get; set; }

        //foreign key
        [Display(Name = "Controladora")]
        public int ControladoraID { get; set; }        
        [ForeignKey("ControladoraID")]
        public virtual Controladora Perfil_Controladora { get; set; }
        //foreign key
        [Display(Name = "Acción")]
        public int AccionID { get; set; }
        [ForeignKey("AccionID")]
        public virtual Accion Perfil_Accion { get; set; }
        //foreign key
        [Display(Name = "Rol")]
        public string RolID { get; set; }
        [ForeignKey("RolID")]
        public virtual ApplicationRole Perfil_Rol { get; set; }
    }

    //AUDITORIA
    public class ActionLog
    {
        public int ActionLogID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IP { get; set; }
        public DateTime DateTime { get; set; }
    }

}