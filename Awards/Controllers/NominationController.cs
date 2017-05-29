using Awards.DAL;
using Awards.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Awards.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NominationController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        [HttpGet]
        public List<GetCategoryDTO> GetNominationsForUser(string user)
        {
            //TODO: Get requesting user e-mail from claims
            //var user = "";

            List<GetCategoryDTO> nominations = db.Categories
                .Where(
                    o => o.Nominees.Any(
                        p => p.Nominations.Any(
                            q => q.Nominator == user
                        )
                    )
                )
                .Select(o => new GetCategoryDTO
                {
                    ID = o.ID,
                    Name = o.Name,
                    Nominees = o.Nominees
                        .Where(p => p.Nominations.Any(q => q.Nominator == user))
                        .Select(p => new GetNomineeDTO
                        {
                            CategoryID = p.CategoryID,
                            NomineeEmail = p.Email,
                            Nominations = p.Nominations
                                .Where(q => q.Nominator == user)
                                .Select(r => new GetNominationDTO
                                {
                                    ID = r.ID,
                                    Anonymous = r.Anonymous,
                                    Nominator = r.Nominator,
                                    NomineeID = r.NomineeID,
                                    Reason = r.Reason
                                }).ToList()
                        }).ToList()

                }).ToList();

            return nominations;
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> AddNomination(SetNomineeDTO nominee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //TODO: Get nominating user from claims
            var nominator = "";
            var existingNominee = db.Nominees.FirstOrDefault(
                o => o.CategoryID == nominee.CategoryID && o.Email == nominee.NomineeEmail);
            if (existingNominee != null)
            {
                if(existingNominee.Nominations.Any(o => o.Nominator == nominator))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    response.ReasonPhrase = "You have already nominated this person in this category";
                    return ResponseMessage(response);
                }
                var newNomination = new Nomination
                {
                    NomineeID = existingNominee.ID,
                    Anonymous = nominee.Nomination.Anonymous,
                    Nominator = nominator,
                    Reason = nominee.Nomination.Reason
                };
                db.Nominations.Add(newNomination);
            }
            else
            {
                var newNominee = new Nominee
                {
                    CategoryID = nominee.CategoryID,
                    Email = nominee.NomineeEmail,
                    Name = "", //TODO: Get name from AD based on Email.
                };
                newNominee.Nominations.Add(new Nomination
                {

                    Anonymous = nominee.Nomination.Anonymous,
                    Nominator = nominator,
                    Reason = nominee.Nomination.Reason
                });

                db.Nominees.Add(newNominee);
            }
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNomination(int id, SetNominationDTO nomination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingNomination = await db.Nominations.FindAsync(id);
            if (existingNomination == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorized(existingNomination.Nominator))
            {
                return Unauthorized();
            }

            var updatedNomination = existingNomination;
            updatedNomination.Anonymous = nomination.Anonymous;
            updatedNomination.Reason = nomination.Reason;

            db.Entry(updatedNomination).State = EntityState.Modified;

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

        // DELETE: api/Nomination
        [ResponseType(typeof(Nomination))]
        public async Task<IHttpActionResult> DeleteNomination(int id)
        {
            Nomination nomination = await db.Nominations.FindAsync(id);
            if (nomination == null)
            {
                return NotFound();
            }
            if (!IsUserAuthorized(nomination.Nominator))
            {
                return Unauthorized();
            }
            db.Nominations.Remove(nomination);
            await db.SaveChangesAsync();
            var nominee = await db.Nominees.FindAsync(nomination.NomineeID);
            if (nominee.Nominations.Count < 1)
            {
                db.Nominees.Remove(nominee);
                await db.SaveChangesAsync();
            }

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

        private bool IsUserAuthorized(string user)
        {
            //TODO get Requesting user email from Claims
            var userEmail = "";
            if (user != userEmail)
            {
                return false;
            }
            return true;
        }
    }
}
