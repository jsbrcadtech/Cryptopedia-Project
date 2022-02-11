namespace CryptopediaWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.Int(nullable: false, identity: true),
                        TokenName = c.String(),
                        TokenCreationYear = c.Int(nullable: false),
                        TokenDescription = c.String(),
                    })
                .PrimaryKey(t => t.TokenID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tokens");
        }
    }
}
