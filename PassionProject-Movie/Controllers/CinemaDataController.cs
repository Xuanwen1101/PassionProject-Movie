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

        // GET: api/CinemaData/ListCinemas
        [HttpGet]
        public IQueryable<Cinema> ListCinemas()
        {
            return db.Cinemas;
        }

        // GET: api/CinemaData/FindCinema/5
        [ResponseType(typeof(Cinema))]
        [HttpGet]
        public IHttpActionResult FindCinema(int id)
        {
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound();
            }

            return Ok(cinema);
        }

        // PUT: api/CinemaData/UpdateCinema/5
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

        // POST: api/CinemaData/AddCinema
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

        // DELETE: api/CinemaData/DeleteCinema/5
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