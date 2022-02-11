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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44311/api/");
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

            string url = "tokendata/findtoken/"+id;
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
            //Information about all networks in the system.
            //Get api/networksdata/listnetworks
            string url = "networksdata/listnetworks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<NetworksDto> NetworksOptions = response.Content.ReadAsAsync<IEnumerable<NetworksDto>>().Result;

            return View(NetworksOptions);
        }

        // POST: Token/Create
        [HttpPost]
        public ActionResult Create(Token token)
        {
            Debug.WriteLine("the jsonpayload is:");
            Debug.WriteLine(token.TokenName);
            //Objective: Add a new token into the system using the API 
            //curl -H "Content-Type:application/json" -d @token.json https://localhost:44311/api/tokendata/addtoken
            string url = "tokendata/addtoken";

            string jsonpayload = jss.Serialize(token);
            Debug.WriteLine(jsonpayload);

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
        public ActionResult Edit(int id)
        {
            UpdateToken ViewModel = new UpdateToken();

            //the existing Token information
            string url = "tokendata/findtoken/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TokenDto SelectedToken = response.Content.ReadAsAsync<TokenDto>().Result;
            ViewModel.SelectedToken = SelectedToken;

            // all networks to choose from when updating this animal
            //the existing token information
            url = "networksdata/listnetworks/";
            response = client.GetAsync(url).Result;
            IEnumerable<NetworksDto> NetworksOptions = response.Content.ReadAsAsync<IEnumerable<NetworksDto>>().Result;

            ViewModel.NetworksOptions = NetworksOptions;

            return View(ViewModel);
        }

        // POST: Token/Update/5
        [HttpPost]
        public ActionResult Update(int id, Token token)
        {

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
        public ActionResult DeleteConfirm(int id)
        {
            string url = "tokendata/findtoken/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TokenDto selectedtoken = response.Content.ReadAsAsync<TokenDto>().Result;
            return View(selectedtoken);
        }

        // GET: Token/Delete/5
        public ActionResult Delete(int id)
        {
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
