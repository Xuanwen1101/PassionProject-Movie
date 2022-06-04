using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Movie.Models.ViewModels
{
    public class DetailsDirector
    {
        public DirectorDto SelectedDirector { get; set; }

        //all of movies that were filmed by the selected Director
        public IEnumerable<MovieDto> FilmedMovies { get; set; }
    }
}