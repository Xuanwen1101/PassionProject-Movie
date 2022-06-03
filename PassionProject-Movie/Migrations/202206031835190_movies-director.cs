namespace PassionProject_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moviesdirector : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "Cinema_CinemaID", "dbo.Cinemas");
            DropIndex("dbo.Movies", new[] { "Cinema_CinemaID" });
            CreateTable(
                "dbo.MovieCinemas",
                c => new
                    {
                        Movie_MovieID = c.Int(nullable: false),
                        Cinema_CinemaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_MovieID, t.Cinema_CinemaID })
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .ForeignKey("dbo.Cinemas", t => t.Cinema_CinemaID, cascadeDelete: true)
                .Index(t => t.Movie_MovieID)
                .Index(t => t.Cinema_CinemaID);
            
            AddColumn("dbo.Movies", "DirectorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Movies", "DirectorID");
            AddForeignKey("dbo.Movies", "DirectorID", "dbo.Directors", "DirectorID", cascadeDelete: true);
            DropColumn("dbo.Movies", "Cinema_CinemaID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Cinema_CinemaID", c => c.Int());
            DropForeignKey("dbo.Movies", "DirectorID", "dbo.Directors");
            DropForeignKey("dbo.MovieCinemas", "Cinema_CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.MovieCinemas", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.MovieCinemas", new[] { "Cinema_CinemaID" });
            DropIndex("dbo.MovieCinemas", new[] { "Movie_MovieID" });
            DropIndex("dbo.Movies", new[] { "DirectorID" });
            DropColumn("dbo.Movies", "DirectorID");
            DropTable("dbo.MovieCinemas");
            CreateIndex("dbo.Movies", "Cinema_CinemaID");
            AddForeignKey("dbo.Movies", "Cinema_CinemaID", "dbo.Cinemas", "CinemaID");
        }
    }
}
