using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/treinamentos")]
    public class TreinamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TreinamentosController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: api/treinamentos
        // ============================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Treinamento>>> Get()
        {
            var lista = await _context.Treinamentos.ToListAsync();
            return Ok(lista);
        }

        // ============================
        // POST: api/treinamentos
        // ============================
        [HttpPost]
        public async Task<ActionResult<Treinamento>> Post(
            [FromBody] CreateTreinamentoDto dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var treinamento = new Treinamento
            {
                Titulo = dto.Titulo,
                Instrutor = dto.Instrutor,
                ImgVideo = dto.ImgVideo,
                VideoURL = dto.VideoURL,
                Categoria = dto.Categoria
            };

            _context.Treinamentos.Add(treinamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = treinamento.Id },
                treinamento
            );
        }
    }
}

