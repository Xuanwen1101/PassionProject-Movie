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

namespace PassionProject_Director.Controllers
{
    public class DirectorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DirectorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }

        // GET: Director/List
        public ActionResult List()
        {
            string url = "DirectorData/ListDirectors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DirectorDto> Director = response.Content.ReadAsAsync<IEnumerable<DirectorDto>>().Result;


            return View(Director);

        }

        // GET: Director/Details/5
        public ActionResult Details(int id)
        {
            DetailsDirector ViewModel = new DetailsDirector();

            string url = "DirectorData/FindDirector/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DirectorDto SelectedDirector = response.Content.ReadAsAsync<DirectorDto>().Result;

            ViewModel.SelectedDirector = SelectedDirector;

            
            url = "MovieData/ListMoviesForDirector/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> filmedMovies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.FilmedMovies = filmedMovies;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Director/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Director/Create
        [HttpPost]
        public ActionResult Create(Director director)
        {
           
            string url = "DirectorData/AddDirector";
            string jsonPayload = jss.Serialize(director);

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

        // GET: Director/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "DirectorData/FindDirector/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DirectorDto selectedDirector = response.Content.ReadAsAsync<DirectorDto>().Result;
            return View(selectedDirector);
        }

        // POST: Director/Update/5
        [HttpPost]
        public ActionResult Update(int id, Director director)
        {
            string url = "DirectorData/UpdateDirector/" + id;
            string jsonPayload = jss.Serialize(director);

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

        // GET: Director/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DirectorData/FindDirector/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DirectorDto selectedDirector = response.Content.ReadAsAsync<DirectorDto>().Result;
            return View(selectedDirector);
        }

        // POST: Director/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DirectorData/DeleteDirector/" + id;
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
