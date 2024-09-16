namespace StudentRegSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntitiesConfiguration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Majors", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Students", "DateofBirth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Students", "Address", c => c.String(maxLength: 255));
            AlterColumn("dbo.Students", "Phone", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Phone", c => c.String());
            AlterColumn("dbo.Students", "Address", c => c.String());
            AlterColumn("dbo.Students", "DateofBirth", c => c.DateTime());
            AlterColumn("dbo.Students", "Name", c => c.String());
            AlterColumn("dbo.Majors", "Name", c => c.String());
        }
    }
}
