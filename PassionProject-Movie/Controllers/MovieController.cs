using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject_Movie.Models;
using System.Web.Script.Serialization;

namespace PassionProject_Movie.Controllers
{
    public class MovieController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MovieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/MovieData/");
        }


        // GET: Movie/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of movies
            //curl https://localhost:44349/api/MovieData/ListMovies

            string url = "ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> movies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with the data api to retrieve the selected movie info
            //curl https://localhost:44349/api/MovieData/FindMovie/{id}

            string url = "FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto selectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;

            return View(selectedMovie);
        }

        // GET: Movie/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {

            //Debug.WriteLine(movie.MovieName);
            //objective: add a new movie into our system using the API
            //curl -H "Content-Type:application/json" -d @movie.json https://localhost:44349/api/MovieData/AddMovie 
            string url = "AddMovie";


            string jsonPayload = jss.Serialize(movie);

            Debug.WriteLine("the json payload is :", jsonPayload);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
