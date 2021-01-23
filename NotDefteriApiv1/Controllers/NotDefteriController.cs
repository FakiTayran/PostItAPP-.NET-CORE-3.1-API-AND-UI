using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotDefteriApiv1.Models;

namespace NotDefteriApiv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotDefteriController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;

        public NotDefteriController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/NotDefteri
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotDefteri>>> GetNotDefteris()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.FindFirst(ClaimTypes.Email).Value;
            AppUser user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
            return await _context.NotDefteris.ToListAsync();
        }

        // GET: api/NotDefteri/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotDefteri>> GetNotDefteri(int id)
        {
            var notDefteri = await _context.NotDefteris.FindAsync(id);

            if (notDefteri == null)
            {
                return NotFound();
            }

            return notDefteri;
        }

        // PUT: api/NotDefteri/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutNotDefteri([FromHeader]NotDefteri not)
        {
            if (not.Id == 0)
            {
                return BadRequest();
            }

            NotDefteri notDefteri = await _context.NotDefteris.FindAsync(not.Id);
            notDefteri.Icerik = not.Icerik;
            notDefteri.Time = DateTime.Now;
            _context.Entry(notDefteri).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotDefteriExists(not.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(notDefteri);
        }

        // POST: api/NotDefteri
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NotDefteri>> PostNotDefteri(string Icerik)
        {
            NotDefteri notDefteri = new NotDefteri();
            notDefteri.Icerik = Icerik;
            notDefteri.Time = DateTime.Now;
            var identity = (ClaimsIdentity)User.Identity;
            var email = identity.FindFirst(ClaimTypes.Email).Value;
            AppUser user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
            await _context.AddAsync(notDefteri);
            await _context.SaveChangesAsync();

            return Ok(notDefteri);
        }

        // DELETE: api/NotDefteri/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NotDefteri>> DeleteNotDefteri(int id)
        {
            var notDefteri = await _context.NotDefteris.FindAsync(id);
            if (notDefteri == null)
            {
                return NotFound();
            }

            _context.NotDefteris.Remove(notDefteri);
            await _context.SaveChangesAsync();

            return notDefteri;
        }

        private bool NotDefteriExists(int id)
        {
            return _context.NotDefteris.Any(e => e.Id == id);
        }
    }
}
