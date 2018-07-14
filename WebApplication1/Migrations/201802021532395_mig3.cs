namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleViewModels", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleViewModels", "Description");
        }
    }
}
