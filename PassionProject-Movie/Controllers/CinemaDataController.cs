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
    public class CinemaDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Cinemas in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Cinemas in the database.
        /// </returns>
        /// <example>
        /// GET: api/CinemaData/ListCinemas
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CinemaDto))]
        public IHttpActionResult ListCinemas()
        {
            List<Cinema> Cinemas = db.Cinemas.ToList();
            List<CinemaDto> CinemaDtos = new List<CinemaDto>();

            Cinemas.ForEach(c => CinemaDtos.Add(new CinemaDto()
            {
                CinemaID = c.CinemaID,
                CinemaName = c.CinemaName,
                CinemaLocation = c.CinemaLocation
            }));

            return Ok(CinemaDtos);
        }


        /// <summary>
        /// Returns all Cinemas in the system associated with the selected movie.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Cinemas in the database showing the selected movie.
        /// </returns>
        /// <param name="id">Movie Primary Key</param>
        /// <example>
        /// GET: api/CinemaData/ListCinemasForMovie/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CinemaDto))]
        public IHttpActionResult ListCinemasForMovie(int id)
        {
            List<Cinema> Cinemas = db.Cinemas.Where(
                c => c.Movies.Any(
                    m => m.MovieID == id)
                ).ToList();
            List<CinemaDto> CinemaDtos = new List<CinemaDto>();

            Cinemas.ForEach(c => CinemaDtos.Add(new CinemaDto()
            {
                CinemaID = c.CinemaID,
                CinemaName = c.CinemaName,
                CinemaLocation = c.CinemaLocation
            }));

            return Ok(CinemaDtos);
        }


        /// <summary>
        /// Returns Cinemas in the system not caring for the selected movie.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Cinemas in the database not showing the selected movie.
        /// </returns>
        /// <param name="id">Movie Primary Key</param>
        /// <example>
        /// GET: api/CinemaData/ListCinemasNotShowingForMovie/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CinemaDto))]
        public IHttpActionResult ListCinemasNotShowingForMovie(int id)
        {
            List<Cinema> Cinemas = db.Cinemas.Where(
                c => !c.Movies.Any(
                    m => m.MovieID == id)
                ).ToList();
            List<CinemaDto> CinemaDtos = new List<CinemaDto>();

            Cinemas.ForEach(c => CinemaDtos.Add(new CinemaDto()
            {
                CinemaID = c.CinemaID,
                CinemaName = c.CinemaName,
                CinemaLocation = c.CinemaLocation
            }));

            return Ok(CinemaDtos);
        }


        /// <summary>
        /// Returns all Cinemas in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Cinema in the system matching up to the Cinema ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Cinema</param>
        /// <example>
        /// GET: api/CinemaData/FindCinema/5
        /// </example>
        [ResponseType(typeof(Cinema))]
        [HttpGet]
        public IHttpActionResult FindCinema(int id)
        {
            Cinema Cinema = db.Cinemas.Find(id);
            CinemaDto CinemaDto = new CinemaDto()
            {
                CinemaID = Cinema.CinemaID,
                CinemaName = Cinema.CinemaName,
                CinemaLocation = Cinema.CinemaLocation
            };
            if (Cinema == null)
            {
                return NotFound();
            }

            return Ok(CinemaDto);
        }


        /// <summary>
        /// Updates a particular Cinema in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Cinema ID primary key</param>
        /// <param name="Cinema">JSON FORM DATA of an Cinema</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/CinemaData/UpdateCinema/5
        /// FORM DATA: Cinema JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCinema(int id, Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cinema.CinemaID)
            {
                return BadRequest();
            }

            db.Entry(cinema).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(id))
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
        /// Adds an Cinema to the system
        /// </summary>
        /// <param name="Cinema">JSON FORM DATA of an Cinema</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Cinema ID, Cinema Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CinemaData/AddCinema
        /// FORM DATA: Cinema JSON Object
        /// </example>
        [ResponseType(typeof(Cinema))]
        [HttpPost]
        public IHttpActionResult AddCinema(Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cinemas.Add(cinema);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cinema.CinemaID }, cinema);
        }


        /// <summary>
        /// Deletes an Cinema from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Cinema</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/CinemaData/DeleteCinema/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Cinema))]
        [HttpPost]
        public IHttpActionResult DeleteCinema(int id)
        {
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound();
            }

            db.Cinemas.Remove(cinema);
            db.SaveChanges();

            return Ok(cinema);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CinemaExists(int id)
        {
            return db.Cinemas.Count(e => e.CinemaID == id) > 0;
        }
    }
}