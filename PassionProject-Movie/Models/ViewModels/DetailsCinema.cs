using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Movie.Models.ViewModels
{
    public class DetailsCinema
    {
        public CinemaDto SelectedCinema { get; set; }
        //all of movies that were shown in the selected Cinema
        public IEnumerable<MovieDto> ShownMovies { get; set; }
    }
}