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
            //client = new HttpClient();
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44349/api/");
        }


        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";

            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Director/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Director director)
        {
            GetApplicationCookie();//get token credentials

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
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "DirectorData/FindDirector/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DirectorDto selectedDirector = response.Content.ReadAsAsync<DirectorDto>().Result;
            return View(selectedDirector);
        }

        // POST: Director/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Director director)
        {
            GetApplicationCookie();//get token credentials

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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DirectorData/FindDirector/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DirectorDto selectedDirector = response.Content.ReadAsAsync<DirectorDto>().Result;
            return View(selectedDirector);
        }

        // POST: Director/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials

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
