using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using SendGrid;
//using SendGrid.SmtpApi;
//using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
//using System.Net;
//using System.Configuration;
//using System.Diagnostics;
//using WebApplication1.HtmlHelperExtensions;
using System.Data.Entity.Migrations;

namespace WebApplication1
{
    public class EmailService : IIdentityMessageService
    {
        //NJB- INI - comentado
        //public async Task SendAsync(IdentityMessage message)
        //{
        // Plug in your email service here to send an email.
        //return Task.FromResult(0);
        //}
        //NJB- FIN - comentado

        //NJB-INI - SendGrid configuration
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            // Retrieve the API key from the environment variables. See the project README for more info about setting this up.
            var apiKey = Environment.GetEnvironmentVariable("api_key_name_NJB1");

            var client = new SendGridClient(apiKey);

            // Send a Single Email using the Mail Helper
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("OpenMindedSoft@hotmail.com", "OpenMinded-Soft Team"),
                Subject = message.Subject,
                PlainTextContent = message.Body,
                HtmlContent = message.Body
            };
            msg.AddTo(message.Destination);

            var response = await client.SendEmailAsync(msg);
            //Console.WriteLine(msg.Serialize());
            //Console.WriteLine(response.StatusCode);
            //Console.WriteLine(response.Headers);
            //Console.WriteLine("\n\nPress <Enter> to continue.");
            //Console.ReadLine();

            //    var myMessage = new SendGridMessage();
            //    myMessage.AddTo(message.Destination);
            //    myMessage.From = new System.Net.Mail.MailAddress("OpenMindSoft@hotmail.com", "SM Motos");
            //    myMessage.Subject = message.Subject;
            //    myMessage.Text = message.Body;
            //    myMessage.Html = message.Body;

            //    var credentials = new NetworkCredential(
            //               ConfigurationManager.AppSettings["mailAccount"],
            //               ConfigurationManager.AppSettings["mailPassword"]
            //               );

            //    // Create a Web transport for sending email.
            //    var transportWeb = new Web(credentials);

            //    // Send the email.
            //    if (transportWeb != null)
            //    {
            //        await transportWeb.DeliverAsync(myMessage);
            //    }
            //    else
            //    {
            //        Trace.TraceError("Failed to create Web transport.");
            //        await Task.FromResult(0);
            //    }
            //}
            //NJB-FIN - SendGrid configuration
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        //NJB-INI - Agregado de uso de roles
        public virtual async Task<IdentityResult> AddUserToRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore
                .GetRolesAsync(user)
                .ConfigureAwait(false);

            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(role => !userRoles.Contains(role)))
            {
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);
            }
            // Call update once when all roles are added
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore
                .GetRolesAsync(user)
                .ConfigureAwait(false);

            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(userRoles.Contains))
            {
                await userRoleStore
                    .RemoveFromRoleAsync(user, role)
                    .ConfigureAwait(false);
            }
            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }
        //NJB-FIN - Agregado de uso de roles
    }

    //NJB-INI - Agregado de uso de roles
    // Configure the RoleManager used in the application.RoleManager is defined in the ASP.NET Identity core assembly  (NJB-Security)

    //****INI-Cambio IdentityRole por --> ApplicationRole debido a la personalización de los roles 
    //public class ApplicationRoleManager : RoleManager<IdentityRole>
    //{
    //    public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
    //        : base(roleStore)
    //    {
    //    }


    //    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    //    {
    //        var manager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

    //        return manager;
    //    }
    //}
    //****FIN-Cambio IdentityRole por --> ApplicationRole debido a la personalización de los roles 

    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }


        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var manager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));

            return manager;
        }
    }
    //NJB-FIN - Agregado de uso de roles

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //CustomAssemblyHelper helper = new CustomAssemblyHelper();
            //var syscontrollersList = helper.GetControllerNames();

            //syscontrollersList

            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "norberto.bordon@hotmail.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                //dentityRole represents an actual Role entity in our application and in the database
                //while IdentityUserRole represents the relationship between a User and a Role
                //NJB-FIN - ApplicationRole instead of IdentityRole
                //role = new IdentityRole(roleName);
                role = new ApplicationRole(roleName);
                //NJB-FIN - ApplicationRole instead of IdentityRole
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                //add empleado
                var empleado = new Empleado
                {
                    PersonaNombre = "Administrador",
                    EmpleadoNivel = "N1",
                    EmpleadoSector = Empleadosector.Empresa,
                    PersonaCUIL = 80000000000,
                    PersonaApellido = "Administrador",
                    PersonaDni = 31999888,
                    PersonaDireccion = "España 1000",
                    PersonaFechaNacimiento = DateTime.Parse("1970-09-10"),
                    PersonaLocalidad = "Rosario",
                    PersonaMail = "admin@mail.com",
                    PersonaNacionalidad = "Argentina",
                    PersonaSexo = "M",
                    PersonaTelefono = 0341156177888,
                    EmpleadoTipo = Empleadotipo.Gerente
                };
                db.Empleados.AddOrUpdate(p => p.PersonaNombre,empleado);
                db.SaveChanges();                

                user = new ApplicationUser { UserName = name, Email = name };

                user.ApplicationUser_Persona = empleado;
                user.FirstName = "Administrador";
                user.LastName = name;

                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);

                //NJB-ini
                empleado.Empleado_AppUser = db.Users.Where(x => x.Id == user.Id).SingleOrDefault();

                db.Empleados.AddOrUpdate(p => p.PersonaNombre, empleado);
                db.SaveChanges();
                //NJB-fin
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            //NJB-INI - Create Role Manager if it does not exist
            var roleMan = roleManager.FindByName("Manager");
            if (roleMan == null)
            {
                roleMan = new ApplicationRole("Manager");

                var roleresult = roleManager.Create(roleMan);
            }

            //NJB-INI - Create Role Employee if it does not exist
            var roleEmp = roleManager.FindByName("Employee");
            if (roleEmp == null)
            {
                roleEmp = new ApplicationRole("Employee");

                var roleresult = roleManager.Create(roleEmp);
            }

            //NJB-INI - INITIALIZE DB  
            var productos = new List<Producto>
            {
                new Producto { ProductoDesc = "Onda Elite 125",   ProductoEstadoActivo = true, ProductoPrecio = 50000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Onda PCX 150",   ProductoEstadoActivo = true, ProductoPrecio = 75000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Motomel Stratto 125",   ProductoEstadoActivo = true, ProductoPrecio = 28000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Lambretta 125",   ProductoEstadoActivo = true, ProductoPrecio = 75000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Motomel Stratto 150",   ProductoEstadoActivo = true, ProductoPrecio = 28000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Piaggio 125",   ProductoEstadoActivo = true, ProductoPrecio = 35000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Motomel Clasica 125",   ProductoEstadoActivo = true, ProductoPrecio = 25000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Kimco 125",   ProductoEstadoActivo = true, ProductoPrecio = 23000, ProductoMoneda = "ARS"},
                new Producto { ProductoDesc = "Kimco 150",   ProductoEstadoActivo = true, ProductoPrecio = 55000, ProductoMoneda = "ARS"}
            };

            productos.ForEach(s => db.Productos.AddOrUpdate(p => p.ProductoDesc, s));
            db.SaveChanges();

            var proveedores = new List<Proveedor>
            {
                new Proveedor { ProveedorNombre = "Motos Argentina", ProveedorCUIT = 00501231230, ProveedorDesc = "Multimarcas Motos" , ProveedorDireccion = "Mitre 100" , ProveedorEstadoActivo = true , ProveedorContacto = "motosArg@motosarg.com.ar" },
                new Proveedor { ProveedorNombre = "Motomel Argentina", ProveedorCUIT = 10501231231, ProveedorDesc = "Oficial Motomel Motos" , ProveedorDireccion = "Mitre 200" , ProveedorEstadoActivo = true , ProveedorContacto = "motomel@motosarg.com.ar"},
                new Proveedor { ProveedorNombre = "Kimco Argentina", ProveedorCUIT = 20501231232, ProveedorDesc = "Oficial Kimco Motos" , ProveedorDireccion = "Mitre 300" , ProveedorEstadoActivo = false , ProveedorContacto = "kimco@motosarg.com.ar"},
                new Proveedor { ProveedorNombre = "Honda Argentina", ProveedorCUIT = 30501231233, ProveedorDesc = "Oficial Honda Motos" , ProveedorDireccion = "Mitre 400" , ProveedorEstadoActivo = false , ProveedorContacto = "Honda@motosarg.com.ar"},
            };
            
            proveedores.ForEach(s => db.Proveedores.AddOrUpdate(p => p.ProveedorNombre, s));
            db.SaveChanges();

            AddOrUpdateProveedor(db, "Motos Argentina", "Onda Elite 125");
            AddOrUpdateProveedor(db, "Motos Argentina", "Motomel Stratto 125");
            AddOrUpdateProveedor(db, "Motos Argentina", "Lambretta 125");
            AddOrUpdateProveedor(db, "Motomel Argentina", "Motomel Stratto 125");
            AddOrUpdateProveedor(db, "Motomel Argentina", "Motomel Clasica 125");
            AddOrUpdateProveedor(db, "Kimco Argentina", "Kimco 125");
            AddOrUpdateProveedor(db, "Kimco Argentina", "Kimco 150");
            AddOrUpdateProveedor(db, "Honda Argentina", "Onda PCX 150");
            AddOrUpdateProveedor(db, "Honda Argentina", "Onda Elite 125");

            db.SaveChanges();

            var cuentas = new List<Cuenta>
            {
                new Cuenta { CuentaDesc = "Cuenta Corriente 000123", CuentaTitular = "Cuenta Proveedor Motos Argentina"},
                new Cuenta { CuentaDesc = "Cuenta Corriente 010123", CuentaTitular = "Cuenta Proveedor Motomel Argentina"},
                new Cuenta { CuentaDesc = "Cuenta Corriente 020123", CuentaTitular = "Cuenta Proveedor Kimco Argentina"},
                new Cuenta { CuentaDesc = "Cuenta Corriente 111111", CuentaTitular = "Cuenta Proveedor Honda Argentina"},
                new Cuenta { CuentaDesc = "Cuenta Corriente 030123", CuentaTitular = "Cuenta propia SM Motos"}
            };

            cuentas.ForEach(s => db.Cuentas.AddOrUpdate(p => p.CuentaDesc, s));
            db.SaveChanges();

            var empresas = new List<Empresa>
            {
                new Empresa { EmpresaDesc = "Do not stop dreaming", EmpresaNombre = "OpenMided Software"}                
            };
            empresas.ForEach(s => db.Empresas.AddOrUpdate(p => p.EmpresaNombre, s));
            db.SaveChanges();

            var sucursales = new List<Sucursal>
            {
                new Sucursal { SucursalDireccion = "España 1180", SucursalNombre = "Sucursal Rosario Centro", EmpresaID = empresas.Single( i => i.EmpresaNombre == "OpenMided Software").EmpresaID}, 
                new Sucursal { SucursalDireccion = "Puccio 1180", SucursalNombre = "Sucursal Rosario Oeste", EmpresaID = empresas.Single( i => i.EmpresaNombre == "OpenMided Software").EmpresaID}, 
                new Sucursal { SucursalDireccion = "Lamadrid 1180", SucursalNombre = "Sucursal Rosario Sur", EmpresaID = empresas.Single(i => i.EmpresaNombre == "OpenMided Software").EmpresaID }
            };
            sucursales.ForEach(s => db.Sucursales.AddOrUpdate(p => p.SucursalNombre, s));
            db.SaveChanges();

            var departamentos = new List<TipoDepartamento>
            {
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Ventas.ToString(), TipoDepartamentoNombre = "Departamento Ventas"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Compras.ToString(), TipoDepartamentoNombre = "Departamento Compras"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Tesoreria.ToString(), TipoDepartamentoNombre = "Departamento Tesoreria"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Taller.ToString(), TipoDepartamentoNombre = "Departamento Taller"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Repuestos.ToString(), TipoDepartamentoNombre = "Departamento Repuestos"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Gestoria.ToString(), TipoDepartamentoNombre = "Departamento Gestoria"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Contabilidad.ToString(), TipoDepartamentoNombre = "Departamento Contabilidad"},
                new TipoDepartamento { TipoDepartamentoDesc = Empleadosector.Finanzas.ToString(), TipoDepartamentoNombre = "Departamento Finanzas"},
            };
            departamentos.ForEach(s => db.TipoDepartamentoes.AddOrUpdate(p => p.TipoDepartamentoDesc, s));
            db.SaveChanges();

            AddOrUpdateDepartamento(db, "Sucursal Rosario Centro", Empleadosector.Ventas.ToString());
            AddOrUpdateDepartamento(db, "Sucursal Rosario Centro", Empleadosector.Taller.ToString());
            AddOrUpdateDepartamento(db, "Sucursal Rosario Centro", Empleadosector.Gestoria.ToString());
            AddOrUpdateDepartamento(db, "Sucursal Rosario Oeste", Empleadosector.Ventas.ToString());
            AddOrUpdateDepartamento(db, "Sucursal Rosario Sur", Empleadosector.Ventas.ToString());

            var clientes = new List<Cliente>
            {
                new Cliente { PersonaNombre = "Cliente1", ClienteClase = Clienteclase.Black, ClienteTipo = Clientetipo.Individual, PersonaCUIL = 10501231200, PersonaApellido = "ApeCli_1",
                              PersonaDni = 30999888, PersonaDireccion = "Italia 1000", PersonaFechaNacimiento = DateTime.Parse("1980-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "persona1@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341156177888},
                new Cliente { PersonaNombre = "Cliente2", ClienteClase = Clienteclase.Gold, ClienteTipo = Clientetipo.Empresa, PersonaCUIL = 20501231200, PersonaApellido = "ApeCli_2",
                              PersonaDni = 500999888, PersonaDireccion = "Italia 2000", PersonaFechaNacimiento = DateTime.Parse("2007-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empresa1@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "F", PersonaTelefono = 0341156177999},
                new Cliente { PersonaNombre = "Cliente3", ClienteClase = Clienteclase.Platinium, ClienteTipo = Clientetipo.Individual_Premium, PersonaCUIL = 30501231200, PersonaApellido = "ApeCli_3",
                              PersonaDni = 30999888, PersonaDireccion = "Italia 3000", PersonaFechaNacimiento = DateTime.Parse("1981-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "persona2@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341156177333},
                new Cliente { PersonaNombre = "Cliente4", ClienteClase = Clienteclase.Standard, ClienteTipo = Clientetipo.Empresa_Premium, PersonaCUIL = 40501231200, PersonaApellido = "ApeCli_4",
                              PersonaDni = 30999888, PersonaDireccion = "Italia 4000", PersonaFechaNacimiento = DateTime.Parse("2010-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empresa2@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "F", PersonaTelefono = 0341156177222},
            };
            clientes.ForEach(s => db.Clientes.AddOrUpdate(p => p.PersonaNombre, s));
            db.SaveChanges();

            var empleados = new List<Empleado>
            {
                new Empleado { PersonaNombre = "Empleado1.0", EmpleadoNivel = "N4", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 50501231200, PersonaApellido = "ApeEmp_1.0",
                              PersonaDni = 31999888, PersonaDireccion = "España 1000", PersonaFechaNacimiento = DateTime.Parse("1970-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado1.0@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341156177888, EmpleadoTipo = Empleadotipo.Operario},
                new Empleado { PersonaNombre = "Empleado1.1", EmpleadoNivel = "N4", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 50501231200, PersonaApellido = "ApeEmp_1.1",
                              PersonaDni = 31999888, PersonaDireccion = "España 1000", PersonaFechaNacimiento = DateTime.Parse("1970-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado1.1@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341156177888, EmpleadoTipo = Empleadotipo.Operario},
                new Empleado { PersonaNombre = "Empleado1.2", EmpleadoNivel = "N4", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 50501231200, PersonaApellido = "ApeEmp_1.2",
                              PersonaDni = 31999888, PersonaDireccion = "España 1000", PersonaFechaNacimiento = DateTime.Parse("1970-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado1.2@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341156177888, EmpleadoTipo = Empleadotipo.Operario},
                new Empleado { PersonaNombre = "Empleado2", EmpleadoNivel = "N3", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 51501231200, PersonaApellido = "ApeEmp_2",
                              PersonaDni = 31999876, PersonaDireccion = "España 2000", PersonaFechaNacimiento = DateTime.Parse("1971-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado2@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "F", PersonaTelefono = 0341156177123, EmpleadoTipo = Empleadotipo.Encargado},
                new Empleado { PersonaNombre = "Empleado3", EmpleadoNivel = "N2", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 52501231200, PersonaApellido = "ApeEmp_3",
                              PersonaDni = 32999876, PersonaDireccion = "España 3000", PersonaFechaNacimiento = DateTime.Parse("1972-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado3@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "F", PersonaTelefono = 0341156177123, EmpleadoTipo = Empleadotipo.Gerente},
                new Empleado { PersonaNombre = "Empleado4", EmpleadoNivel = "N1", EmpleadoSector = Empleadosector.Ventas, PersonaCUIL = 53501231200, PersonaApellido = "ApeEmp_4",
                              PersonaDni = 30999876, PersonaDireccion = "España 4000", PersonaFechaNacimiento = DateTime.Parse("1973-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "empleado4@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341153337123, EmpleadoTipo = Empleadotipo.Socio}
            };
            empleados.ForEach(s => db.Empleados.AddOrUpdate(p => p.PersonaNombre, s));
            db.SaveChanges();

            var prospectos = new List<Prospecto>
            {
                new Prospecto { PersonaNombre = "Prospecto1",  ProspectoCanalInicial = "Facebook",  ProspectoConocimientoTecnico = true, PersonaCUIL = 55501231200, PersonaApellido = "ApeProsp_1",
                              PersonaDni = 30999876, PersonaDireccion = "España 1000", PersonaFechaNacimiento = DateTime.Parse("1953-09-10"), PersonaLocalidad = "Rosario",
                              PersonaMail = "prospecto1@mail.com", PersonaNacionalidad = "Argentina", PersonaSexo = "M", PersonaTelefono = 0341153322123,  ProspectoProfesion = "Ingeniero en Sistemas"},
            };
            prospectos.ForEach(s => db.Prospectos.AddOrUpdate(p => p.PersonaNombre, s));
            db.SaveChanges();
            //NJB-FIN - INITIALIZE DB  
        }

        public static void AddOrUpdateProveedor(ApplicationDbContext context, string proveedorNombre, string productoDesc)
        {
            var prod = context.Productos.SingleOrDefault(c => c.ProductoDesc == productoDesc);
            var prov = prod.Producto_Proveedores.SingleOrDefault(i => i.ProveedorNombre == proveedorNombre);
            if (prov == null)
                prod.Producto_Proveedores.Add(context.Proveedores.Single(i => i.ProveedorNombre == proveedorNombre));
        }

        public static void AddOrUpdateDepartamento(ApplicationDbContext context, string SucursalNombre, string DeptoDesc)
        {
            var sucu = context.Sucursales.SingleOrDefault(c => c.SucursalNombre == SucursalNombre);
            var dept = sucu.Sucursal_TipoDepartamentos.SingleOrDefault(i => i.TipoDepartamentoDesc == DeptoDesc);
            if (dept == null)
                sucu.Sucursal_TipoDepartamentos.Add(context.TipoDepartamentoes.Single(i => i.TipoDepartamentoDesc == DeptoDesc));
        }
    }
}
