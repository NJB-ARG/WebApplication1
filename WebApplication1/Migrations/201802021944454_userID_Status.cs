namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userID_Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Solicituds", "OwnerID", c => c.String());
            //AlterColumn("dbo.Solicituds", "SolicitudEstado", c => c.Int(nullable: false));
            AlterColumn("dbo.Solicituds", "SolicitudEstado", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Solicituds", "SolicitudEstado", c => c.String());
            DropColumn("dbo.Solicituds", "OwnerID");
        }
    }
}
