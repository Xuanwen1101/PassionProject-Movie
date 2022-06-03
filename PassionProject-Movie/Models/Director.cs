using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PassionProject_Movie.Models
{
    public class Director
    {
        [Key]
        public int DirectorID { get; set; }

        public string DirectorFName { get; set; }
        public string DirectorLName { get; set; }
        public string DirectorBio { get; set; }

    }
}