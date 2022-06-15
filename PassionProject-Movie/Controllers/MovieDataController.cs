using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject_Movie.Models;
using System.Diagnostics;

namespace PassionProject_Movie.Controllers
{
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Movies in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Movies in the database.
        /// </returns>
        /// <example>
        /// GET: api/MovieData/ListMovies
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMovies()
        {
            List<Movie> Movies = db.Movies.ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(m => MovieDtos.Add(new MovieDto()
            {
                MovieID = m.MovieID,
                MovieName = m.MovieName,
                MovieIntro = m.MovieIntro,
                ReleaseDate = m.ReleaseDate,
                MovieHasPic = m.MovieHasPic,
                PicExtension = m.PicExtension,
                DirectorID = m.Director.DirectorID,
                DirectorFName = m.Director.DirectorFName,
                DirectorLName = m.Director.DirectorLName
            }));


            return Ok(MovieDtos);

        }

        /// <summary>
        /// Gathers information about all Movies related to the selected Director ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Movies in the database matched with the selected Director ID
        /// </returns>
        /// <param name="id">Director ID.</param>
        /// <example>
        /// GET: api/MovieData/ListMoviesForDirector/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMoviesForDirector(int id)
        {
            List<Movie> Movies = db.Movies.Where(m => m.DirectorID == id).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(m => MovieDtos.Add(new MovieDto()
            {
                MovieID = m.MovieID,
                MovieName = m.MovieName,
                MovieIntro = m.MovieIntro,
                ReleaseDate = m.ReleaseDate,
                MovieHasPic = m.MovieHasPic,
                PicExtension = m.PicExtension,
                DirectorID = m.Director.DirectorID,
                DirectorFName = m.Director.DirectorFName,
                DirectorLName = m.Director.DirectorLName
            }));

            return Ok(MovieDtos);
        }

        /// <summary>
        /// Gathers information about Movies related to the selected Cinema ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Movies in the database matched to the selected Cinema ID
        /// </returns>
        /// <param name="id">Cinema ID.</param>
        /// <example>
        /// GET: api/MovieData/ListMoviesForCinema/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMoviesForCinema(int id)
        {
            //all Movies that have Cinemas which match with our ID
            List<Movie> Movies = db.Movies.Where(
                m => m.Cinemas.Any(
                    c => c.CinemaID == id
                )).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(m => MovieDtos.Add(new MovieDto()
            {
                MovieID = m.MovieID,
                MovieName = m.MovieName,
                MovieIntro = m.MovieIntro,
                ReleaseDate = m.ReleaseDate,
                MovieHasPic = m.MovieHasPic,
                PicExtension = m.PicExtension,
                DirectorID = m.Director.DirectorID,
                DirectorFName = m.Director.DirectorFName,
                DirectorLName = m.Director.DirectorLName
            }));

            return Ok(MovieDtos);
        }


        /// <summary>
        /// Associates a particular cinema with a particular movie
        /// </summary>
        /// <param name="movieId">The movie ID primary key</param>
        /// <param name="cinemaId">The cinema ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/AssociateMovieWithCinema/9/1
        /// </example>
        [HttpPost]
        [Route("api/MovieData/AssociateMovieWithCinema/{movieId}/{cinemaId}")]
        [Authorize]
        public IHttpActionResult AssociateMovieWithCinema(int movieId, int cinemaId)
        {

            Movie SelectedMovie = db.Movies.Include(m => m.Cinemas).Where(m => m.MovieID == movieId).FirstOrDefault();
            Cinema SelectedCinema = db.Cinemas.Find(cinemaId);

            if (SelectedMovie == null || SelectedCinema == null)
            {
                return NotFound();
            }


            SelectedMovie.Cinemas.Add(SelectedCinema);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular cinema and a particular movie
        /// </summary>
        /// <param name="movieId">The movie ID primary key</param>
        /// <param name="cinemaId">The cinema ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/AssociateMovieWithCinema/9/1
        /// </example>
        [HttpPost]
        [Route("api/MovieData/UnAssociateMovieWithCinema/{movieId}/{cinemaId}")]
        [Authorize]
        public IHttpActionResult UnAssociateMovieWithCinema(int movieId, int cinemaId)
        {

            Movie SelectedMovie = db.Movies.Include(m => m.Cinemas).Where(m => m.MovieID == movieId).FirstOrDefault();
            Cinema SelectedCinema = db.Cinemas.Find(cinemaId);

            if (SelectedMovie == null || SelectedCinema == null)
            {
                return NotFound();
            }


            SelectedMovie.Cinemas.Remove(SelectedCinema);
            db.SaveChanges();

            return Ok();
        }



        /// <summary>
        /// Returns all movies in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: The movie in the system matching up to the movie ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the movie</param>
        /// <example>
        /// GET: api/MovieData/FindMovie/5
        /// </example>
        [ResponseType(typeof(MovieDto))]
        [HttpGet]
        public IHttpActionResult FindMovie(int id)
        {
            Movie Movie = db.Movies.Find(id);
            MovieDto MovieDto = new MovieDto()
            {
                MovieID = Movie.MovieID,
                MovieName = Movie.MovieName,
                MovieIntro = Movie.MovieIntro,
                ReleaseDate = Movie.ReleaseDate,
                MovieHasPic = Movie.MovieHasPic,
                PicExtension = Movie.PicExtension,
                DirectorID = Movie.Director.DirectorID,
                DirectorFName = Movie.Director.DirectorFName,
                DirectorLName = Movie.Director.DirectorLName
            };
            if (Movie == null)
            {
                return NotFound();
            }

            return Ok(MovieDto);
        }


        /// <summary>
        /// Updates the selected movie in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Movie ID primary key</param>
        /// <param name="movie">JSON FORM DATA of an movie</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/MovieData/UpdateMovie/5
        /// FORM DATA: Movie JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.MovieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(movie).Property(m => m.MovieHasPic).IsModified = false;
            db.Entry(movie).Property(m => m.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Receives movie picture data, uploads it to the webserver and updates the movie's HasPic option
        /// </summary>
        /// <param name="id">the movie id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F MoviePicture=@file.jpg "https://localhost:44349/api/MovieData/UploadMoviePicture/5"
        /// POST: api/MovieData/UploadMoviePicture/5
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        [Authorize]
        public IHttpActionResult UploadMoviePicture(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                //Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                //Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var moviePicture = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (moviePicture.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(moviePicture.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Images/Movies/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Movies/"), fn);

                                //save the file
                                moviePicture.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the movie haspic and picextension fields in the database
                                Movie SelectedMovie = db.Movies.Find(id);
                                SelectedMovie.MovieHasPic = haspic;
                                SelectedMovie.PicExtension = extension;
                                db.Entry(SelectedMovie).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Movie Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();
            }
        }


        /// <summary>
        /// Adds a Movie to the system
        /// </summary>
        /// <param name="movie">JSON FORM DATA of a Movie</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Movie ID, Movie Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/MovieData/AddMovie
        /// FORM DATA: Movie JSON Object
        /// </example>
        [ResponseType(typeof(Movie))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.MovieID }, movie);
        }


        /// <summary>
        /// Deletes the selected Movie from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Movie</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/MovieData/DeleteMovie/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Movie))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            if (movie.MovieHasPic && movie.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Movies/" + id + "." + movie.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.MovieID == id) > 0;
        }
    }
}