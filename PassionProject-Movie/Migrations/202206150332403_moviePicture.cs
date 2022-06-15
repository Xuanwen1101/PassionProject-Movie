namespace PassionProject_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moviePicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MovieHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "PicExtension");
            DropColumn("dbo.Movies", "MovieHasPic");
        }
    }
}
