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
using System.Web.Http.Cors;

namespace Awards.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VotesController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // GET: api/Votes
        public IEnumerable<Vote> GetVotes()
        {
            return db.Votes.ToList();
        }

        // GET: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<IHttpActionResult> GetVote(int id)
        {
            Vote vote = await db.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // PUT: api/Votes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVote(int id, Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.ID)
            {
                return BadRequest();
            }

            db.Entry(vote).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostVote(string user, SetVoteDTO vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUser = ""; //Get from Claims
            var nominee = await db.Nominees.FindAsync(vote.NomineeID);
            if(nominee.Votes.Any(o => o.Voter == currentUser))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                response.ReasonPhrase = "You have already voted for a nominee in this category";
                return ResponseMessage(response);
            }

            db.Votes.Add(new Vote
            {
                NomineeID = vote.NomineeID,
                Voter = currentUser
            });
            await db.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<IHttpActionResult> DeleteVote(string user, int id)
        {
            Vote vote = await db.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            if (vote.Voter != user)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                response.ReasonPhrase = "You cannot delete other peoples votes.";
                return ResponseMessage(response);
            }

            db.Votes.Remove(vote);
            await db.SaveChangesAsync();

            return Ok(vote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoteExists(int id)
        {
            return db.Votes.Count(e => e.ID == id) > 0;
        }
    }
}