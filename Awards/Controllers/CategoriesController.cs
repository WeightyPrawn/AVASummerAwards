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
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        private AwardsContext db = new AwardsContext();

        // GET: api/Categories
        /* public IEnumerable<GetCategoryDTO> GetCategories()
        {
            var categories = db.Categories.Select(s => new GetCategoryDTO
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                Nominees = s.Nominees.Select(t => new GetNomineeDTO
                {
                    CategoryID = t.CategoryID,
                    NomineeEmail = t.Email,
                    Nominations = t.Nominations.Select(u => new GetNominationDTO
                    {
                        Anonymous = u.Anonymous,
                        ID = u.ID,
                        Nominator = u.Nominator,
                        NomineeID = u.NomineeID,
                        Reason = u.Reason
                    })
                })
            });
            return categories.ToList();
        }*/
        // GET: api/Categories
        public IEnumerable<GetCategoryDTO> GetCategories(string user)
        {
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
                                NomineeEmail = p.Email,
                                NomineeName = p.Name,
                                Nominations = p.Nominations
                                    .Select(r => new GetNominationDTO
                                    {
                                        ID = r.ID,
                                        Anonymous = r.Anonymous,
                                        Nominator = r.Nominator,
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
            return response;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
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