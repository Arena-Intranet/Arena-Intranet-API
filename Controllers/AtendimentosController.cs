using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/atendimento")]
    public class AtendimentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AtendimentoController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: api/atendimento
        // Pendentes primeiro
        // ============================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _context.Atendimentos
                .OrderByDescending(a => a.Status == "Pendente")
                .ThenByDescending(a => a.DataAbertura)
                .ToListAsync();

            return Ok(lista);
        }

        // ============================
        // POST: api/atendimento
        // ============================
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AtendimentoCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos");

            var atendimento = new Atendimento
            {
                Numero = dto.Numero,
                Nome = dto.Nome,
                Titulo = dto.Titulo,
                Setor = dto.Setor,
                Mensagem = dto.Mensagem,
                Empresa =dto.Empresa,
                Status = "Pendente",
                DataAbertura = DateTime.Now
            };

            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            return Ok(atendimento); 
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string novoStatus)
        {
            // Busca o atendimento no banco
            var atendimento = await _context.Atendimentos.FindAsync(id);

            if (atendimento == null)
                return NotFound("Atendimento não encontrado.");

            // Validação básica (opcional): impede status vazios
            if (string.IsNullOrWhiteSpace(novoStatus))
                return BadRequest("O status não pode ser vazio.");

            // Atualiza apenas o campo necessário
            atendimento.Status = novoStatus;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(atendimento);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro ao atualizar o status no banco de dados.");
            }
        }
    }

}
