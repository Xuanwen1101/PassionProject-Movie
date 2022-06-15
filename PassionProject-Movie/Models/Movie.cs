using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject_Movie.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        public string MovieName { get; set; }

        public string MovieIntro { get; set; }
        public DateTime ReleaseDate { get; set; }

        //data needed for keeping track of Movie images uploaded
        //images deposited into /Content/Images/Movies/{id}.{extension}
        public bool MovieHasPic { get; set; }
        public string PicExtension { get; set; }


        //A movie is created by one director.
        //A director can make many movies.
        [ForeignKey("Director")]
        public int DirectorID { get; set; }
        public virtual Director Director { get; set; }

        //A movie can be shown by many cinema.
        public ICollection<Cinema> Cinemas { get; set; }
    }

    public class MovieDto
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string MovieIntro { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int DirectorID { get; set; }
        public string DirectorFName { get; set; }
        public string DirectorLName { get; set; }

        //data needed for keeping track of Movies images uploaded
        //images deposited into /Content/Images/Movies/{id}.{extension}
        public bool MovieHasPic { get; set; }
        public string PicExtension { get; set; }

    }

}