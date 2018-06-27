namespace MountainAddicted.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GCodeConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HeightRequestDbDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MountainId = c.Int(nullable: false),
                        NeLat = c.Decimal(nullable: false, precision: 14, scale: 10),
                        NeLng = c.Decimal(nullable: false, precision: 14, scale: 10),
                        SwLat = c.Decimal(nullable: false, precision: 14, scale: 10),
                        SwLng = c.Decimal(nullable: false, precision: 14, scale: 10),
                        ResolutionX = c.Int(nullable: false),
                        ResolutionY = c.Int(nullable: false),
                        RequestNumber = c.Int(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MountainDbDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NeLat = c.Decimal(nullable: false, precision: 14, scale: 10),
                        NeLng = c.Decimal(nullable: false, precision: 14, scale: 10),
                        SwLat = c.Decimal(nullable: false, precision: 14, scale: 10),
                        SwLng = c.Decimal(nullable: false, precision: 14, scale: 10),
                        Title = c.String(),
                        Description = c.String(),
                        PreviewData = c.String(),
                        OriginalData = c.String(),
                        GCodeData = c.String(),
                        GCodeConfig_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GCodeConfigs", t => t.GCodeConfig_Id)
                .Index(t => t.GCodeConfig_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MountainDbDatas", "GCodeConfig_Id", "dbo.GCodeConfigs");
            DropIndex("dbo.MountainDbDatas", new[] { "GCodeConfig_Id" });
            DropTable("dbo.MountainDbDatas");
            DropTable("dbo.HeightRequestDbDatas");
            DropTable("dbo.GCodeConfigs");
        }
    }
}
