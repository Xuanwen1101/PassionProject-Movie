using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject_Movie.Models;
using PassionProject_Movie.Models.ViewModels;
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
            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }

        /// GET: Movie/List
        public ActionResult List()
        {
            //objective: communicate with the data api to retrieve a list of movies
            //curl https://localhost:44349/api/MovieData/ListMovies

            string url = "MovieData/ListMovies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> movies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(movies);
        }


        


        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            DetailsMovie ViewModel = new DetailsMovie();

            //objective: communicate with the data api to retrieve the selected movie info
            //curl https://localhost:44349/api/MovieData/FindMovie/{id}

            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto selectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;

            ViewModel.SelectedMovie = selectedMovie;

            //show associated cinemas with this movie
            url = "CinemaData/ListCinemasForMovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CinemaDto> showingCinemas = response.Content.ReadAsAsync<IEnumerable<CinemaDto>>().Result;

            ViewModel.ShowingCinemas = showingCinemas;

            url = "CinemaData/ListCinemasNotCaringForMovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CinemaDto> availableCinemas = response.Content.ReadAsAsync<IEnumerable<CinemaDto>>().Result;

            ViewModel.AvailableCinemas = availableCinemas;


            return View(ViewModel);

        }

        //POST: Movie/Associate/{movieId}
        [HttpPost]
        public ActionResult Associate(int id, int CinemaID)
        {
            //Debug.WriteLine("Attempting to associate movie :" + id + " with cinema " + CinemaID);

            //associate movie with cinema
            string url = "MovieData/AssociateMovieWithCinema/" + id + "/" + CinemaID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Movie/UnAssociate/{id}?CinemaID={cinemaID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int CinemaID)
        {
            //Debug.WriteLine("Attempting to unassociate movie :" + id + " with cinema: " + CinemaID);

            //unassociate movie with cinema
            string url = "MovieData/UnAssociateMovieWithCinema/" + id + "/" + CinemaID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Movie/New
        public ActionResult New()
        {
            string url = "DirectorData/ListDirectors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DirectorDto> DirectorOptions = response.Content.ReadAsAsync<IEnumerable<DirectorDto>>().Result;

            return View(DirectorOptions);
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {

            //objective: add a new movie into our system using the API
            //curl -H "Content-Type:application/json" -d @movie.json https://localhost:44349/api/MovieData/AddMovie 
            string url = "MovieData/AddMovie";


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
            UpdateMovie ViewModel = new UpdateMovie();

            //get the existing movie information
            //curl https://localhost:44349/api/MovieData/FindMovie/{id}
            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto SelectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;
            ViewModel.SelectedMovie = SelectedMovie;

            //all directors to choose from when updating this movie
            //the existing Director Options
            url = "DirectorData/ListDirector/";
            response = client.GetAsync(url).Result;
            IEnumerable<DirectorDto> DirectorOptions = response.Content.ReadAsAsync<IEnumerable<DirectorDto>>().Result;

            ViewModel.DirectorOptions = DirectorOptions;

            return View(ViewModel);
        }

        // POST: Movie/Update/5
        [HttpPost]
        public ActionResult Update(int id, Movie movie)
        {
            //objective: update the selected movie into our system using the API
            //curl -H "Content-Type:application/json" -d @movie.json  https://localhost:44349/api/MovieData/UpdateMovie/{id}
            string url = "MovieData/UpdateMovie/" + id;
            string jsonPayload = jss.Serialize(movie);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // get the existing movie information
            //curl https://localhost:44349/api/MovieData/FindMovie/{id}
            string url = "MovieData/FindMovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto selectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;

            return View(selectedMovie);

        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: delete the selected movie from our system using the API
            //curl -d "" https://localhost:44349/api/MovieData/DeleteMovie/{id}
            string url = "MovieData/DeleteMovie/" + id;
            HttpContent content = new StringContent("");
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
    }
}
