namespace nfconlab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionItems",
                c => new
                    {
                        QuestionItemId = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer1 = c.String(),
                        Answer2 = c.String(),
                        Answer3 = c.String(),
                        Answer4 = c.String(),
                        Location = c.String(),
                        Date = c.String(),
                    })
                .PrimaryKey(t => t.QuestionItemId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QuestionItems");
        }
    }
}
