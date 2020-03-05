namespace EnterpriseFinancialApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PAtest : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Transactions", name: "AccountId", newName: "PersonalAccountId");
            RenameIndex(table: "dbo.Transactions", name: "IX_AccountId", newName: "IX_PersonalAccountId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Transactions", name: "IX_PersonalAccountId", newName: "IX_AccountId");
            RenameColumn(table: "dbo.Transactions", name: "PersonalAccountId", newName: "AccountId");
        }
    }
}
