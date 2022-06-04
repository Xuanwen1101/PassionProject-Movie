using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PassionProject_Movie.Models
{
    public class Cinema
    {
        [Key]
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string CinemaLocation { get; set; }


        //A cinema can show many movies.
        public ICollection<Movie> Movies { get; set; }
    }

    public class CinemaDto
    {
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string CinemaLocation { get; set; }
    }

}