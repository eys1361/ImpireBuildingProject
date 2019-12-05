using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpireBuilding.Data;
using ImpireBuilding.Models;

namespace ImpireBuilding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIVisitorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public APIVisitorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/APIVisitors
        [HttpGet]
        public IEnumerable<Visitor> GetVisitor()
        {
            return _context.Visitor;
        }

        // GET: api/APIVisitors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var visitor = await _context.Visitor.FindAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }

            return Ok(visitor);
        }

        // PUT: api/APIVisitors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitor([FromRoute] int id, [FromBody] Visitor visitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != visitor.Id)
            {
                return BadRequest();
            }

            _context.Entry(visitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/APIVisitors
        [HttpPost]
        public async Task<IActionResult> PostVisitor([FromBody] Visitor visitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Visitor.Add(visitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisitor", new { id = visitor.Id }, visitor);
        }

        // DELETE: api/APIVisitors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var visitor = await _context.Visitor.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }

            _context.Visitor.Remove(visitor);
            await _context.SaveChangesAsync();

            return Ok(visitor);
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitor.Any(e => e.Id == id);
        }
    }
}