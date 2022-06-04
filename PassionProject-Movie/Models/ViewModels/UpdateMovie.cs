using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Movie.Models.ViewModels
{
    public class UpdateMovie
    {
        public MovieDto SelectedMovie { get; set; }

        public IEnumerable<DirectorDto> DirectorOptions { get; set; }
    }
}