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
using CryptopediaWebApp.Models;
using System.Diagnostics;

namespace CryptopediaWebApp.Controllers
{
    public class NetworksDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Networks in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all networks in the database
        /// </returns>
        /// <example>
        /// GET: api/NetworksData/ListNetworks
        /// </example>
        [HttpGet]
        [ResponseType(typeof(NetworksDto))]
        public IHttpActionResult ListNetworks()
        {
            List<Networks> Networks = db.Networks.ToList();
            List<NetworksDto> NetworksDtos = new List<NetworksDto>();

            Networks.ForEach(s => NetworksDtos.Add(new NetworksDto()
            {
                NetworksID = s.NetworksID,
                NetworksName = s.NetworksName,
                NetworksStandard = s.NetworksStandard
            }));

            return Ok(NetworksDtos);
        }

        /// <summary>
        /// Returns all Networks in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Network in the system matching up to the Networks ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Network</param>
        /// <example>
        /// GET: api/NetworksData/FindNetworks/5
        /// </example>
        [ResponseType(typeof(NetworksDto))]
        [HttpGet]
        public IHttpActionResult FindNetworks(int id)
        {
            Networks Networks = db.Networks.Find(id);
            NetworksDto NetworksDto = new NetworksDto()
            {
                NetworksID = Networks.NetworksID,
                NetworksName = Networks.NetworksName,
                NetworksStandard = Networks.NetworksStandard
            };
            if (Networks == null)
            {
                return NotFound();
            }

            return Ok(NetworksDto);
        }

        /// <summary>
        /// Updates a Networks in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Networks ID primary key</param>
        /// <param name="Networks">JSON FORM DATA of a Networks</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/NetworksData/UpdateNetworks/2
        /// FORM DATA: Networks JSON Object
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateNetworks(int id, Networks Networks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Networks.NetworksID)
            {

                return BadRequest();
            }

            db.Entry(Networks).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworksExists(id))
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
        /// Adds an Networks to the system
        /// </summary>
        /// <param name="Networks">JSON FORM DATA of a Networks</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Networks ID, Networks Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/NetworksData/Networks
        /// FORM DATA: Networks JSON Object
        /// </example>
        [ResponseType(typeof(Networks))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddNetworks(Networks Networks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Networks.Add(Networks);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Networks.NetworksID }, Networks);
        }

        /// <summary>
        /// Deletes a Networks from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Networks</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/NetworksData/DeleteNetworks/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Networks))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteNetworks(int id)
        {
            Networks Networks = db.Networks.Find(id);
            if (Networks == null)
            {
                return NotFound();
            }

            db.Networks.Remove(Networks);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NetworksExists(int id)
        {
            return db.Networks.Count(e => e.NetworksID == id) > 0;
        }
    }
}