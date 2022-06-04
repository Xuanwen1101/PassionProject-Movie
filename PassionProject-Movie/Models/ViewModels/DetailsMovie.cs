using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Movie.Models.ViewModels
{
    public class DetailsMovie
    {
        public MovieDto SelectedMovie { get; set; }

        //all of cinemas that are showing the selected Movie
        public IEnumerable<CinemaDto> ShowingCinemas { get; set; }

        public IEnumerable<CinemaDto> AvailableCinemas { get; set; }
    }
}