using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    //NJB-Model agregado para Seguridad (Security)
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        //NJB- Added custom prop to roles
        public string Description { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        // NJB - Add more fields
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [Display(Name = "Provincia")]
        public string State { get; set; }
        [Display(Name = "Código Postal")]
        public string PostalCode { get; set; }
        //

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}