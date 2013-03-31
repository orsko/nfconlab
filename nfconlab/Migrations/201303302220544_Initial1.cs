namespace nfconlab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionItems", "RightAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionItems", "RightAnswer");
        }
    }
}
