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

namespace CryptopediaWebApp.Controllers
{
    public class TokenDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TokenData/ListTokens
        [HttpGet]
        public IEnumerable<TokenDto> ListTokens()
        {
            List<Token> Tokens = db.Tokens.ToList();
            List<TokenDto> TokenDtos = new List<TokenDto>();

            Tokens.ForEach(a => TokenDtos.Add(new TokenDto()
            {
                TokenID = a.TokenID,
                TokenName = a.TokenName,
                TokenCreationYear = a.TokenCreationYear,
                TokenDescription = a.TokenDescription,
                NetworksID = a.Networks.NetworksID,
                NetworksName = a.Networks.NetworksName,
                NetworksStandard = a.Networks.NetworksStandard
            }));
            return TokenDtos;
        }

        /// <summary>
        /// Gathers information about all tokens related to a particular networks ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all tokens in the database, including their associated networks matched with a particular networks ID
        /// </returns>
        /// <param name="id">Networks ID.</param>
        /// <example>
        /// GET: api/TokenData/ListTokensForNetworks/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TokenDto))]
        public IHttpActionResult ListTokensForNetworks(int id)
        {
            List<Token> Tokens = db.Tokens.Where(a => a.NetworksID == id).ToList();
            List<TokenDto> TokenDtos = new List<TokenDto>();

            Tokens.ForEach(a => TokenDtos.Add(new TokenDto()
            {
                TokenID = a.TokenID,
                TokenName = a.TokenName,
                TokenCreationYear = a.TokenCreationYear,
                TokenDescription = a.TokenDescription,
                NetworksID = a.Networks.NetworksID,
                NetworksName = a.Networks.NetworksName,
                NetworksStandard = a.Networks.NetworksStandard
            }));

            return Ok(TokenDtos);
        }

        // GET: api/TokenData/FindToken/5
        [ResponseType(typeof(Token))]
        [HttpGet]
        public IHttpActionResult FindToken(int id)
        {
            Token Token = db.Tokens.Find(id);
            TokenDto TokenDto = new TokenDto()
            {
                TokenID = Token.TokenID,
                TokenName = Token.TokenName,
                TokenCreationYear = Token.TokenCreationYear,
                TokenDescription = Token.TokenDescription,
                NetworksID = Token.Networks.NetworksID,
                NetworksName = Token.Networks.NetworksName,
                NetworksStandard = Token.Networks.NetworksStandard
            };
            if (Token == null)
            {
                return NotFound();
            }

            return Ok(TokenDto);
        }

        // Post: api/TokenData/UpdateToken/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateToken(int id, Token token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != token.TokenID)
            {
                return BadRequest();
            }

            db.Entry(token).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
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

        // POST: api/TokenData/AddToken
        [HttpPost]
        [ResponseType(typeof(Token))]
        [Authorize]
        public IHttpActionResult AddToken(Token token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tokens.Add(token);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = token.TokenID }, token);
        }

        /// <summary>
        /// Deletes an token from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the token</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TokenData/DeleteToken/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Token))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteToken(int id)
        {
            Token token = db.Tokens.Find(id);
            if (token == null)
            {
                return NotFound();
            }

            db.Tokens.Remove(token);
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

        private bool TokenExists(int id)
        {
            return db.Tokens.Count(e => e.TokenID == id) > 0;
        }
    }
}