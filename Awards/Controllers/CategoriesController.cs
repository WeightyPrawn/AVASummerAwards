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
using System.Security.Claims;
using System.Text.RegularExpressions;
using Awards.Helpers;

namespace Awards.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // GET: api/Categories
        [ResponseType(typeof(GetCategoryDTO))]
        public async Task<IHttpActionResult> GetCategories()
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if(!AuthHelper.IsValidUserDomain(user))
            {
                return Unauthorized();
            }
            // var name = ClaimsPrincipal.Current.FindFirst("name").Value;
            List<GetCategoryDTO> response = db.Categories
                    .Select(o => new GetCategoryDTO
                    {
                        ID = o.ID,
                        Name = o.Name,
                        Description = o.Description,
                        Nominees = o.Nominees
                            .Select(p => new GetNomineeDTO
                            {
                                ID = p.ID,
                                CategoryID = p.CategoryID,
                                Email = p.Email,
                                Name = p.Name,
                                Image = p.Image,
                                Nominations = p.Nominations
                                    .Select(r => new GetNominationDTO
                                    {
                                        ID = r.ID,
                                        NomineeID = r.NomineeID,
                                        Reason = r.Reason
                                    }).ToList(),
                                Vote = p.Votes
                                    .Where(q => q.Voter == user)
                                    .Select(r => new GetVoteDTO
                                    {
                                        ID = r.ID,
                                        NomineeID = r.NomineeID,
                                        Voter = r.Voter
                                    }).FirstOrDefault()
                            }).ToList()
                    }).ToList();
            response.ForEach(o => o.HasVoted = o.Nominees.Any(p => p.Vote != null));
            response.ForEach(o => o.Nominees = o.Nominees.Shuffle());
            return Ok(response);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if (!AuthHelper.IsAdminUser(user))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.ID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if (!AuthHelper.IsAdminUser(user))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category.ID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            var user = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            if (!AuthHelper.IsAdminUser(user))
            {
                return Unauthorized();
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.ID == id) > 0;
        }
    }
}