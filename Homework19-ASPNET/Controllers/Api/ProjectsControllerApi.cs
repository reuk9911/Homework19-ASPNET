using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Homework19_ASPNET;
using Homework19_ASPNET.Data;

namespace Homework19_ASPNET.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsControllerApi : ControllerBase
    {
        private readonly Homework19_ASPNETContext _context;

        public ProjectsControllerApi(Homework19_ASPNETContext context)
        {
            _context = context;
        }

        // GET: api/ProjectsControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            return await _context.Project.ToListAsync();
        }

        // GET: api/ProjectsControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/ProjectsControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ID)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/ProjectsControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ID }, project);
        }

        // DELETE: api/ProjectsControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }
    }
}
