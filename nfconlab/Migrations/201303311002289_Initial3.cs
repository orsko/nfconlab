namespace nfconlab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionItems", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionItems", "Image");
        }
    }
}
