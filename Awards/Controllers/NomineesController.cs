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
    public class NomineesController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // GET: api/Nominees
        public IEnumerable<Nominee> GetNominees()
        {
            return db.Nominees.ToList();
        }

        // GET: api/Nominees/5
        [ResponseType(typeof(Nominee))]
        public async Task<IHttpActionResult> GetNominee(int id)
        {
            Nominee nominee = await db.Nominees.FindAsync(id);
            if (nominee == null)
            {
                return NotFound();
            }

            return Ok(nominee);
        }

        // PUT: api/Nominees/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNominee(int id, Nominee nominee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nominee.ID)
            {
                return BadRequest();
            }

            db.Entry(nominee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NomineeExists(id))
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

        // POST: api/Nominees
        [ResponseType(typeof(Nominee))]
        public async Task<IHttpActionResult> PostNominee(Nominee nominee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Nominees.Add(nominee);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nominee.ID }, nominee);
        }

        // DELETE: api/Nominees/5
        [ResponseType(typeof(Nominee))]
        public async Task<IHttpActionResult> DeleteNominee(int id)
        {
            Nominee nominee = await db.Nominees.FindAsync(id);
            if (nominee == null)
            {
                return NotFound();
            }

            db.Nominees.Remove(nominee);
            await db.SaveChangesAsync();

            return Ok(nominee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NomineeExists(int id)
        {
            return db.Nominees.Count(e => e.ID == id) > 0;
        }
    }
}