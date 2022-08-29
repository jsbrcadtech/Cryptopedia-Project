using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using CryptopediaWebApp.Models;
using CryptopediaWebApp.Models.ViewModels;
using System.Web.Script.Serialization;

namespace CryptopediaWebApp.Controllers
{
    public class TokenController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static TokenController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            //client.BaseAddress = new Uri("https://localhost:44311/api/");

            //client.BaseAddress = new Uri("http://jsbrcad-001-site1.dtempurl.com/api/");

            //Host Somee
            client.BaseAddress = new Uri("http://cryptopedia.somee.com/api/");



        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //Gets token as it's submitted to the controller
            //Uses it to pass over to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Token/List
        public ActionResult List()
        {
            //Objective: Communicate with token data api to retrieve a list of tokens
            //curl https://localhost:44311/api/tokendata/listtokens

            string url = "tokendata/listtokens";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<TokenDto> tokens = response.Content.ReadAsAsync<IEnumerable<TokenDto>>().Result;
            //Debug.WriteLine("Number of tokens received");
            //Debug.WriteLine(tokens.Count());

            return View(tokens);
        }

        // GET: Token/Details/5
        public ActionResult Details(int id)
        {
            //Objective: Communicate with token data api to retrieve one token
            //curl https://localhost:44311/api/tokendata/findtoken/{id}

            DetailsToken ViewModel = new DetailsToken();

            string url = "tokendata/findtoken/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response is");
            //Debug.WriteLine(response.StatusCode);

            TokenDto SelectedToken = response.Content.ReadAsAsync<TokenDto>().Result;
            //Debug.WriteLine("token received: ");
            //Debug.WriteLine(SelectedToken.TokenName);
            ViewModel.SelectedToken = SelectedToken;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Token/New
        public ActionResult New()
        {
            UpdateToken ViewModel = new UpdateToken();


            string url = "networksdata/listnetworks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<NetworksDto> NetworksOptions = response.Content.ReadAsAsync<IEnumerable<NetworksDto>>().Result;

            ViewModel.NetworksOptions = NetworksOptions;
            return View(ViewModel);
        }

        // POST: Token/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Token token)
        {
            GetApplicationCookie();
            //Debug.WriteLine("the jsonpayload is:");
            //Debug.WriteLine(token.TokenName);
            //Objective: Add a new token into the system using the API 

            string url = "tokendata/addtoken";



            string jsonpayload = jss.Serialize(token);
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Token/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateToken ViewModel = new UpdateToken();

            string url = "tokendata/findtoken/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TokenDto SelectedToken = response.Content.ReadAsAsync<TokenDto>().Result;
            ViewModel.SelectedToken = SelectedToken;

            // all networks to choose from when updating this token
            //the existing token information
            url = "networksdata/listnetworks/";
            response = client.GetAsync(url).Result;
            IEnumerable<NetworksDto> NetworksOptions = response.Content.ReadAsAsync<IEnumerable<NetworksDto>>().Result;
            ViewModel.NetworksOptions = NetworksOptions;

            return View(ViewModel);
        }

        // POST: Token/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Token token)
        {
            GetApplicationCookie(); //Gets authentication token credentials 
            string url = "tokendata/updatetoken/" + id;
            string jsonpayload = jss.Serialize(token);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Token/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "tokendata/findtoken/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TokenDto selectedtoken = response.Content.ReadAsAsync<TokenDto>().Result;
            return View(selectedtoken);
        }

        // POST: Token/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie(); //Gets authentication token credentials 
            string url = "tokendata/deletetoken/" + id;
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
