using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/comunicados")]
    public class ComunicadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComunicadosController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: api/comunicados
        // ============================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comunicado>>> Get()
        {
            var lista = await _context.Comunicados
                .OrderByDescending(c => c.DataPostagem)
                .ToListAsync();

            return Ok(lista);
        }

        // ============================
        // POST: api/comunicados
        // ============================
        [HttpPost]
        public async Task<ActionResult<Comunicado>> Post(
            [FromBody] ComunicadoCreateDto dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comunicado = new Comunicado
            {
                Titulo = dto.Titulo,
                Categoria = dto.Categoria,
                Urgencia = dto.Urgencia,
                Conteudo = dto.Conteudo,
                DataPostagem = DateTime.Now
            };

            _context.Comunicados.Add(comunicado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = comunicado.Id },
                comunicado
            );
        }
        // ============================
        // DELETE: api/comunicados/{id}
        // ============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comunicado = await _context.Comunicados.FindAsync(id);

            if (comunicado == null)
                return NotFound();

            _context.Comunicados.Remove(comunicado);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
