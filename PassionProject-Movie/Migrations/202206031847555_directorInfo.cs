namespace PassionProject_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class directorInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Directors", "SpeciesEndangered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "SpeciesEndangered", c => c.Boolean(nullable: false));
        }
    }
}
