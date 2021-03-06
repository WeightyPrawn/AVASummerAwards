﻿using System;
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
using System.Security.Claims;
using Awards.Helpers;

namespace Awards.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VotesController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // POST: api/Votes
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostVote(SetVoteDTO vote)
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if (!AuthHelper.IsValidUserDomain(user))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nominee = await db.Nominees.FindAsync(vote.NomineeID);
            if (nominee.Votes.Any(o => o.Voter == user))
            {
                var response = Request.CreateResponse(
                    HttpStatusCode.Forbidden,
                    "You have already voted for a nominee in this category"
                    );
                return ResponseMessage(response);
            }

            db.Votes.Add(new Vote
            {
                NomineeID = vote.NomineeID,
                Voter = user
            });
            await db.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Votes/5
        [ResponseType(typeof(Vote))]
        public async Task<IHttpActionResult> DeleteVote(int id)
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if (!AuthHelper.IsValidUserDomain(user))
            {
                return Unauthorized();
            }
            Vote vote = await db.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            if (vote.Voter != user)
            {
                var response = Request.CreateResponse(
                    HttpStatusCode.Forbidden,
                    "You cannot delete other peoples votes."
                    );
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