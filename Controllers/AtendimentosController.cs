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
        // ============================
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _context.Atendimentos
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.Status == "Pendente")
                .ThenByDescending(a => a.DataAbertura)
                .Select(a => new
                {
                    a.Id,
                    a.Numero,
                    a.Titulo,
                    a.Setor,
                    a.Empresa,
                    a.Mensagem,
                    a.Status,
                    a.DataAbertura,
                    a.DataTermino,
                    a.UsuarioId,
                    NomeUsuario = a.Usuario.Nome
                })
                .ToListAsync();

            return Ok(lista);
        }

        // ============================
        // POST: api/atendimento
        // ============================
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AtendimentoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔍 valida se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return BadRequest("Usuário inválido.");

            var atendimento = new Atendimento
            {
                Numero = dto.Numero,
                UsuarioId = dto.UsuarioId,
                Titulo = dto.Titulo,
                Setor = dto.Setor,
                Mensagem = dto.Mensagem,
                Empresa = dto.Empresa,
                Status = "Pendente",
                DataAbertura = DateTime.Now,
                DataTermino = null
            };

            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            return Ok(atendimento);
        }

        // ============================
        // PUT: api/atendimento/{id}/status
        // ============================

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string novoStatus)
        {
            var atendimento = await _context.Atendimentos
                .Include(a => a.Usuario) 
                .FirstOrDefaultAsync(a => a.Id == id);

            if (atendimento == null) return NotFound("Atendimento não encontrado");

            string statusLimpo = novoStatus.Replace("\"", "").Trim();

            if (statusLimpo == "Resolvido")
            {
                atendimento.Status = "Resolvido";
                atendimento.DataTermino = DateTime.Now;
            }
            else if (statusLimpo == "Em atendimento")
            {
                atendimento.Status = "Em atendimento";
                atendimento.DataTermino = null;
            }
            else
            {
                atendimento.Status = "Pendente";
                atendimento.DataTermino = null;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    atendimento.Id,
                    atendimento.Numero,
                    atendimento.UsuarioId,
                    NomeUsuario = atendimento.Usuario?.Nome ?? string.Empty,
                    atendimento.Titulo,
                    atendimento.Setor,
                    atendimento.Empresa,
                    atendimento.Mensagem,
                    atendimento.Status,
                    atendimento.DataAbertura,
                    atendimento.DataTermino
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
