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
    public class CinemaController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CinemaController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }

        // GET: Cinema/List
        public ActionResult List()
        {
            string url = "CinemaData/ListCinemas";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CinemaDto> Cinema = response.Content.ReadAsAsync<IEnumerable<CinemaDto>>().Result;


            return View(Cinema);
        }

        // GET: Cinema/Details/5
        public ActionResult Details(int id)
        {
            DetailsCinema ViewModel = new DetailsCinema();

            string url = "CinemaData/FindCinema/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CinemaDto SelectedCinema = response.Content.ReadAsAsync<CinemaDto>().Result;

            ViewModel.SelectedCinema = SelectedCinema;


            url = "MovieData/ListMoviesForCinema/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> shownMovies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.ShownMovies = shownMovies;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Cinema/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Cinema/Create
        [HttpPost]
        public ActionResult Create(Cinema cinema)
        {
            string url = "CinemaData/AddCinema";
            string jsonPayload = jss.Serialize(cinema);

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

        // GET: Cinema/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "CinemaData/FindCinema/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CinemaDto selectedCinema = response.Content.ReadAsAsync<CinemaDto>().Result;
            return View(selectedCinema);
        }

        // POST: Cinema/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Cinema cinema)
        {
            string url = "CinemaData/UpdateCinema/" + id;
            string jsonPayload = jss.Serialize(cinema);

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

        // GET: Cinema/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CinemaData/FindCinema/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CinemaDto selectedCinema = response.Content.ReadAsAsync<CinemaDto>().Result;
            return View(selectedCinema);
        }

        // POST: Cinema/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "CinemaData/DeleteCinema/" + id;
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
