using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Awards.DAL;
using Awards.Models;

namespace Awards.Controllers
{
    public class NominationsController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // GET: api/Nominations
        public IEnumerable<Nomination> GetNominations()
        {
            return db.Nominations.ToList();
        }

        // GET: api/Nominations/5
        [ResponseType(typeof(Nomination))]
        public async Task<IHttpActionResult> GetNomination(int id)
        {
            Nomination nomination = await db.Nominations.FindAsync(id);
            if (nomination == null)
            {
                return NotFound();
            }

            return Ok(nomination);
        }

        // PUT: api/Nominations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNomination(int id, Nomination nomination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nomination.ID)
            {
                return BadRequest();
            }

            db.Entry(nomination).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NominationExists(id))
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

        // POST: api/Nominations
        [ResponseType(typeof(Nomination))]
        public async Task<IHttpActionResult> PostNomination(Nomination nomination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Nominations.Add(nomination);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nomination.ID }, nomination);
        }

        // DELETE: api/Nominations/5
        [ResponseType(typeof(Nomination))]
        public async Task<IHttpActionResult> DeleteNomination(int id)
        {
            Nomination nomination = await db.Nominations.FindAsync(id);
            if (nomination == null)
            {
                return NotFound();
            }

            db.Nominations.Remove(nomination);
            await db.SaveChangesAsync();

            return Ok(nomination);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NominationExists(int id)
        {
            return db.Nominations.Count(e => e.ID == id) > 0;
        }
    }
}