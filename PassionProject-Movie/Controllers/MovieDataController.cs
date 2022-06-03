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

        // GET: api/MovieData/ListMovies
        [HttpGet]
        public IEnumerable<MovieDto> ListMovies()
        {
            List<Movie> Movies = db.Movies.ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieName = a.MovieName,
                MovieIntro = a.MovieIntro,
                ReleaseDate = a.ReleaseDate,
                DirectorFName = a.Director.DirectorFName,
                DirectorLName = a.Director.DirectorLName
            }));


            return MovieDtos;

        }

        // GET: api/MovieData/FindMovie/5
        [ResponseType(typeof(Movie))]
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
                DirectorFName = Movie.Director.DirectorFName,
                DirectorLName = Movie.Director.DirectorLName
            };
            if (Movie == null)
            {
                return NotFound();
            }

            return Ok(MovieDto);
        }

        // PUT: api/MovieData/UpdateMovie/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/MovieData/AddMovie
        [ResponseType(typeof(Movie))]
        [HttpPost]
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

        // DELETE: api/MovieData/DeleteMovie/5
        [ResponseType(typeof(Movie))]
        [HttpPost]
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