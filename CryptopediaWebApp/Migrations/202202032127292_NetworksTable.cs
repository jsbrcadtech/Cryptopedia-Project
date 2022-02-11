namespace CryptopediaWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NetworksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Networks",
                c => new
                    {
                        NetworksID = c.Int(nullable: false, identity: true),
                        NetworksName = c.String(),
                        NetworksStandard = c.String(),
                    })
                .PrimaryKey(t => t.NetworksID);
            
            AddColumn("dbo.Tokens", "NetworksID", c => c.Int(nullable: false));
            CreateIndex("dbo.Tokens", "NetworksID");
            AddForeignKey("dbo.Tokens", "NetworksID", "dbo.Networks", "NetworksID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "NetworksID", "dbo.Networks");
            DropIndex("dbo.Tokens", new[] { "NetworksID" });
            DropColumn("dbo.Tokens", "NetworksID");
            DropTable("dbo.Networks");
        }
    }
}
