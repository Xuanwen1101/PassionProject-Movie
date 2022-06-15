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
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Cinema/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Cinema cinema)
        {
            GetApplicationCookie();//get token credentials

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
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "CinemaData/FindCinema/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CinemaDto selectedCinema = response.Content.ReadAsAsync<CinemaDto>().Result;
            return View(selectedCinema);
        }

        // POST: Cinema/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Cinema cinema)
        {
            GetApplicationCookie();//get token credentials

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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CinemaData/FindCinema/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CinemaDto selectedCinema = response.Content.ReadAsAsync<CinemaDto>().Result;
            return View(selectedCinema);
        }

        // POST: Cinema/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials

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
