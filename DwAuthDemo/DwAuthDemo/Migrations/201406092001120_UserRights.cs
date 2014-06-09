namespace DwAuthDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRights : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectRights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Project_Id = c.Int(),
                        Right_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Rights", t => t.Right_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Right_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectRights", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectRights", "Right_Id", "dbo.Rights");
            DropForeignKey("dbo.ProjectRights", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectRights", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectRights", new[] { "Right_Id" });
            DropIndex("dbo.ProjectRights", new[] { "Project_Id" });
            DropTable("dbo.Rights");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectRights");
        }
    }
}
