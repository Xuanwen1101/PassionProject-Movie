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

        /// <summary>
        /// Returns all Directors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Directors in the database, including the associated Director.
        /// </returns>
        /// <example>
        /// GET: api/DirectorData/ListDirectors
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DirectorDto))]
        public IHttpActionResult ListDirectors()
        {
            List<Director> Director = db.Directors.ToList();
            List<DirectorDto> DirectorDtos = new List<DirectorDto>();

            Director.ForEach(d => DirectorDtos.Add(new DirectorDto()
            {
                DirectorID = d.DirectorID,
                DirectorFName = d.DirectorFName,
                DirectorLName = d.DirectorLName,
                DirectorBio = d.DirectorBio
            }));

            return Ok(DirectorDtos);
        }


        /// <summary>
        /// Returns all Directors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Director in the system matching up to the Director ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Director</param>
        /// <example>
        /// GET: api/DirectorData/FindDirector/5
        /// </example>
        [ResponseType(typeof(Director))]
        [HttpGet]
        public IHttpActionResult FindDirector(int id)
        {
            Director Director = db.Directors.Find(id);
            DirectorDto DirectorDto = new DirectorDto()
            {
                DirectorID = Director.DirectorID,
                DirectorFName = Director.DirectorFName,
                DirectorLName = Director.DirectorLName,
                DirectorBio = Director.DirectorBio
            };
            if (Director == null)
            {
                return NotFound();
            }

            return Ok(DirectorDto);
        }


        /// <summary>
        /// Updates a particular Director in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Director ID primary key</param>
        /// <param name="Director">JSON FORM DATA of an Director</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// PUT: api/DirectorData/UpdateDirector/5
        /// FORM DATA: Director JSON Object
        /// </example>
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


        /// <summary>
        /// Adds an Director to the system
        /// </summary>
        /// <param name="Director">JSON FORM DATA of an Director</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Director ID, Director Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DirectorData/AddDirector
        /// FORM DATA: Director JSON Object
        /// </example>
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


        /// <summary>
        /// Deletes an Director from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Director</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/DirectorData/DeleteDirector/5
        /// FORM DATA: (empty)
        /// </example>
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