using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/organograma")]
    public class OrganogramaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrganogramaController(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET - ORGANOGRAMA COMPLETO (HIERÁRQUICO)
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetOrganograma()
        {
            var vinculos = await _context.Organograma
                .Include(o => o.Liderado)
                .ToListAsync();

            if (!vinculos.Any())
                return Ok(null);

            var mapa = vinculos
                .Select(o => o.Liderado)
                .Distinct()
                .ToDictionary(
                    u => u.Id,
                    u => new OrganogramaDto
                    {
                        UsuarioId = u.Id,
                        Nome = u.Nome,
                        Cargo = u.Cargo,
                        Setor = u.Setor
                    }
                );

            OrganogramaDto? raiz = null;

            foreach (var item in vinculos)
            {
                if (item.GestorId == null)
                {
                    // CEO
                    raiz = mapa[item.LideradoId];
                }
                else
                {
                    mapa[item.GestorId.Value]
                        .Subordinados
                        .Add(mapa[item.LideradoId]);
                }
            }

            return Ok(raiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNome(
            int id,
            [FromBody] OrganogramaNomeUpdateDto dto)
        {
            if (id != dto.UsuarioId)
                return BadRequest("ID do usuário inválido.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            usuario.Nome = dto.Nome;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
