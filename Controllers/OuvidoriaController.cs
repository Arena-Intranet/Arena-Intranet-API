using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/ouvidoria")]
    public class OuvidoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OuvidoriaController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: api/ouvidoria
        // ============================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ouvidoria>>> Get()
        {
            return await _context.Ouvidoria.ToListAsync();
        }

        // ============================
        // POST: api/ouvidoria
        // ============================

        [HttpPost]
        public async Task<ActionResult<Ouvidoria>> Post([FromBody] OuvidoriaCreateDto dto)
        {
            if (dto == null) return BadRequest();

            var novaOuvidoria = new Ouvidoria
            {
                DataEnvio = DateTime.UtcNow,
                Categoria = dto.Categoria,
                Assunto = dto.Assunto,
                Mensagem = dto.Mensagem,
                Anonimo = dto.Anonimo,
                AutorId = dto.Anonimo == true ? null : dto.AutorId,
                AutorNome = dto.Anonimo == true ? "Anônimo" : dto.AutorNome
            };

            _context.Ouvidoria.Add(novaOuvidoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = novaOuvidoria.Id }, novaOuvidoria);
        }

        // ============================
        // DELETE: api/ouvidoria
        // ============================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Ouvidoria.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = "Registro não encontrado." });
            }

            _context.Ouvidoria.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
