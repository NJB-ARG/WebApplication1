namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cuentas",
                c => new
                    {
                        CuentaID = c.Int(nullable: false, identity: true),
                        SolicitudID = c.Int(nullable: false),
                        CuentaDesc = c.String(),
                    })
                .PrimaryKey(t => t.CuentaID);
            
            CreateTable(
                "dbo.Solicituds",
                c => new
                    {
                        SolicitudID = c.Int(nullable: false, identity: true),
                        SolicitudNum = c.Int(nullable: false),
                        SolicitudDescripcion = c.String(),
                        EmpleadoID = c.Int(),
                        ProspectoID = c.Int(),
                        ClienteID = c.Int(),
                        SolicitudTipoSolicitante = c.String(nullable: false),
                        SolicitudFecCreacion = c.DateTime(nullable: false),
                        SolicitudFecVencimiento = c.DateTime(nullable: false),
                        SolicitudSucursal = c.String(),
                        SolicitudEstado = c.String(),
                        CuentaID = c.Int(),
                        SolicitudMontoTotal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SolicitudID)
                .ForeignKey("dbo.Personas", t => t.ClienteID)
                .ForeignKey("dbo.Cuentas", t => t.CuentaID)
                .ForeignKey("dbo.Personas", t => t.EmpleadoID)
                .ForeignKey("dbo.Personas", t => t.ProspectoID)
                .Index(t => t.EmpleadoID)
                .Index(t => t.ProspectoID)
                .Index(t => t.ClienteID)
                .Index(t => t.CuentaID);
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        PersonaID = c.Int(nullable: false, identity: true),
                        PersonaNombre = c.String(),
                        PersonaApellido = c.String(),
                        PersonaTelefono = c.Long(nullable: false),
                        PersonaDireccion = c.String(),
                        PersonaCUIL = c.Long(nullable: false),
                        PersonaDni = c.Long(nullable: false),
                        PersonaMail = c.String(),
                        PersonaFechaNacimiento = c.DateTime(nullable: false),
                        PersonaSexo = c.String(),
                        PersonaNacionalidad = c.String(),
                        PersonaLocalidad = c.String(),
                        EmpleadoSector = c.String(),
                        EmpleadoTipo = c.Int(),
                        EmpleadoTipoDesc = c.String(),
                        EmpleadoNivel = c.String(),
                        ProspectoUnidadBuscada = c.String(),
                        ProspectoProfesion = c.String(),
                        ProspectoMail2 = c.String(),
                        ProspectoTelefono2 = c.String(),
                        ProspectoFacebook = c.String(),
                        ProspectoTwiter = c.String(),
                        ProspectoHorarioContacto = c.String(),
                        ProspectoCanalInicial = c.String(),
                        ProspectoUsuarioDeMoto = c.Boolean(),
                        ProspectoUnidadActual = c.String(),
                        ProspectoEntregaUnidad = c.Boolean(),
                        ProspectoAnioUnidad = c.String(),
                        ProspectoKmUnidad = c.String(),
                        ProspectoConocimientoTecnico = c.Boolean(),
                        ProspectoTipoUsoUnidad = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PersonaID);
            
            CreateTable(
                "dbo.LineaSolicituds",
                c => new
                    {
                        LineaSolicitudID = c.Int(nullable: false, identity: true),
                        SolicitudID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        LineaSolicitudNum = c.Int(nullable: false),
                        LineaSolicitudPUprod = c.Double(nullable: false),
                        LineaSolicitudCantidad = c.Int(nullable: false),
                        LineaSolicitudMoneda = c.String(),
                        LineaSolicitudMonto = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.LineaSolicitudID)
                .ForeignKey("dbo.Productoes", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.Solicituds", t => t.SolicitudID, cascadeDelete: true)
                .Index(t => t.SolicitudID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        ProductoID = c.Int(nullable: false, identity: true),
                        ProductoDesc = c.String(),
                        ProductoPrecio = c.Double(nullable: false),
                        ProductoEstado = c.String(),
                    })
                .PrimaryKey(t => t.ProductoID);
            
            CreateTable(
                "dbo.Ordens",
                c => new
                    {
                        OrdenID = c.Int(nullable: false, identity: true),
                        SolicitudID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrdenID)
                .ForeignKey("dbo.Solicituds", t => t.SolicitudID, cascadeDelete: true)
                .Index(t => t.SolicitudID);
            
            CreateTable(
                "dbo.Paginas",
                c => new
                    {
                        PaginaID = c.Int(nullable: false, identity: true),
                        PaginaNumero = c.Int(nullable: false),
                        PaginaNombre = c.String(),
                        SolicitudID = c.Int(nullable: false),
                        DatosContactoNombre = c.String(),
                        DatosOrdenTipo = c.String(),
                        DatosOrdenTitulo = c.String(),
                        DatosSolicitudTipo = c.String(),
                        DatosSolicitudTitulo = c.String(),
                        OfertasMercadoConsecionario = c.Boolean(),
                        OfertasMercadoLugar = c.String(),
                        OfertasMercadoFecha = c.DateTime(),
                        OfertasMercadoUnidad = c.String(),
                        OfertasMercadoPrecio = c.Double(),
                        OfertasMercadoFinanciacion = c.String(),
                        OfertasMercadoFinValidez = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Orden_OrdenID = c.Int(),
                        Prospecto_PersonaID = c.Int(),
                    })
                .PrimaryKey(t => t.PaginaID)
                .ForeignKey("dbo.Solicituds", t => t.SolicitudID, cascadeDelete: true)
                .ForeignKey("dbo.Ordens", t => t.Orden_OrdenID)
                .ForeignKey("dbo.Personas", t => t.Prospecto_PersonaID)
                .Index(t => t.SolicitudID)
                .Index(t => t.Orden_OrdenID)
                .Index(t => t.Prospecto_PersonaID);
            
            CreateTable(
                "dbo.Documentoes",
                c => new
                    {
                        DocumentoID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.DocumentoID);
            
            CreateTable(
                "dbo.HojaProspectoViewModels",
                c => new
                    {
                        HojaProspectoViewModelID = c.Int(nullable: false, identity: true),
                        HojaProspectoFecCreacion = c.DateTime(nullable: false),
                        HojaProspectoFecUltMod = c.DateTime(nullable: false),
                        HojaProspectoComentario = c.String(),
                        HojaProspectoProspecto_PersonaID = c.Int(),
                    })
                .PrimaryKey(t => t.HojaProspectoViewModelID)
                .ForeignKey("dbo.Personas", t => t.HojaProspectoProspecto_PersonaID)
                .Index(t => t.HojaProspectoProspecto_PersonaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HojaProspectoViewModels", "HojaProspectoProspecto_PersonaID", "dbo.Personas");
            DropForeignKey("dbo.Solicituds", "ProspectoID", "dbo.Personas");
            DropForeignKey("dbo.Paginas", "Prospecto_PersonaID", "dbo.Personas");
            DropForeignKey("dbo.Paginas", "Orden_OrdenID", "dbo.Ordens");
            DropForeignKey("dbo.Paginas", "SolicitudID", "dbo.Solicituds");
            DropForeignKey("dbo.Ordens", "SolicitudID", "dbo.Solicituds");
            DropForeignKey("dbo.LineaSolicituds", "SolicitudID", "dbo.Solicituds");
            DropForeignKey("dbo.LineaSolicituds", "ProductoID", "dbo.Productoes");
            DropForeignKey("dbo.Solicituds", "EmpleadoID", "dbo.Personas");
            DropForeignKey("dbo.Solicituds", "CuentaID", "dbo.Cuentas");
            DropForeignKey("dbo.Solicituds", "ClienteID", "dbo.Personas");
            DropIndex("dbo.HojaProspectoViewModels", new[] { "HojaProspectoProspecto_PersonaID" });
            DropIndex("dbo.Paginas", new[] { "Prospecto_PersonaID" });
            DropIndex("dbo.Paginas", new[] { "Orden_OrdenID" });
            DropIndex("dbo.Paginas", new[] { "SolicitudID" });
            DropIndex("dbo.Ordens", new[] { "SolicitudID" });
            DropIndex("dbo.LineaSolicituds", new[] { "ProductoID" });
            DropIndex("dbo.LineaSolicituds", new[] { "SolicitudID" });
            DropIndex("dbo.Solicituds", new[] { "CuentaID" });
            DropIndex("dbo.Solicituds", new[] { "ClienteID" });
            DropIndex("dbo.Solicituds", new[] { "ProspectoID" });
            DropIndex("dbo.Solicituds", new[] { "EmpleadoID" });
            DropTable("dbo.HojaProspectoViewModels");
            DropTable("dbo.Documentoes");
            DropTable("dbo.Paginas");
            DropTable("dbo.Ordens");
            DropTable("dbo.Productoes");
            DropTable("dbo.LineaSolicituds");
            DropTable("dbo.Personas");
            DropTable("dbo.Solicituds");
            DropTable("dbo.Cuentas");
        }
    }
}
