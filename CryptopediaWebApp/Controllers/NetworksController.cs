using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using CryptopediaWebApp.Models;
using CryptopediaWebApp.Models.ViewModels;
using System.Web.Script.Serialization;

namespace CryptopediaWebApp.Controllers
{
    public class NetworksController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static NetworksController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44311/api/");
        }

        // GET: Networks/List
        public ActionResult List()
        {
            //objective: communicate with our Networks data api to retrieve a list of Networks
            //curl https://localhost:44324/api/Networksdata/listNetworkss


            string url = "networksdata/listnetworks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<NetworksDto> Networks = response.Content.ReadAsAsync<IEnumerable<NetworksDto>>().Result;
            //Debug.WriteLine("Number of Networks received : ");
            //Debug.WriteLine(Networks.Count());


            return View(Networks);
        }


        // GET: Networks/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Networks data api to retrieve one Networks
            //curl https://localhost:44324/api/Networksdata/findnetworks/{id}

            DetailsNetworks ViewModel = new DetailsNetworks();

            string url = "networksdata/findnetworks/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            NetworksDto SelectedNetworks = response.Content.ReadAsAsync<NetworksDto>().Result;
            //Debug.WriteLine("Networks received : ");
            //Debug.WriteLine(SelectedNetworks.NetworksName);

            ViewModel.SelectedNetworks = SelectedNetworks;

            //showcase information about tokens related to this networks
            //send a request to gather information about tokens related to a particular networks ID
            url = "tokendata/listtokensfornetworks/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TokenDto> RelatedTokens = response.Content.ReadAsAsync<IEnumerable<TokenDto>>().Result;

            ViewModel.RelatedTokens = RelatedTokens;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Networks/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Species/Create
        [HttpPost]
        public ActionResult Create(Networks Networks)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Networks.NetworksName);
            //objective: add a new Networks into our system using the API
            //curl -H "Content-Type:application/json" -d @Networks.json  https://localhost:44324/api/Networksdata/addNetworks 
            string url = "networksdata/addnetworks";


            string jsonpayload = jss.Serialize(Networks);
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

        // GET: Networks/Edit/2
        public ActionResult Edit(int id)
        {
            string url = "networksdata/findnetworks/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            NetworksDto selectedNetworks = response.Content.ReadAsAsync<NetworksDto>().Result;
            return View(selectedNetworks);
        }

        // POST: Networks/Update/2
        [HttpPost]
        public ActionResult Update(int id, Networks Networks)
        {

            string url = "networksdata/updatenetworks/" + id;
            string jsonpayload = jss.Serialize(Networks);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            ////Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Networks/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "networksdata/findnetworks/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            NetworksDto selectedNetworks = response.Content.ReadAsAsync<NetworksDto>().Result;
            return View(selectedNetworks);
        }

        // POST: Networks/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "networksdata/deletenetworks/" + id;
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
