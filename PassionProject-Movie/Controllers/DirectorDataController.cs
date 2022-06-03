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
    public class DirectorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DirectorData/ListDirectors
        [HttpGet]
        public IQueryable<Director> ListDirectors()
        {
            return db.Directors;
        }

        // GET: api/DirectorData/FindDirector/5
        [ResponseType(typeof(Director))]
        [HttpGet]
        public IHttpActionResult FindDirector(int id)
        {
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return NotFound();
            }

            return Ok(director);
        }

        // PUT: api/DirectorData/UpdateDirector/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDirector(int id, Director director)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != director.DirectorID)
            {
                return BadRequest();
            }

            db.Entry(director).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(id))
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

        // POST: api/DirectorData/AddDirector
        [ResponseType(typeof(Director))]
        [HttpPost]
        public IHttpActionResult AddDirector(Director director)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Directors.Add(director);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = director.DirectorID }, director);
        }

        // DELETE: api/DirectorData/DeleteDirector/5
        [ResponseType(typeof(Director))]
        [HttpPost]
        public IHttpActionResult DeleteDirector(int id)
        {
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return NotFound();
            }

            db.Directors.Remove(director);
            db.SaveChanges();

            return Ok(director);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DirectorExists(int id)
        {
            return db.Directors.Count(e => e.DirectorID == id) > 0;
        }
    }
}