using System;
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