using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIArenaAuto.DTOs;
using APIArenaAuto.Data;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioLogin == dto.Usuario);

            if (usuario == null)
                return Unauthorized("Usuário ou senha inválidos");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);

            if (!senhaValida)
                return Unauthorized("Usuário ou senha inválidos");

            return Ok(new
            {
                usuario.Id,
                usuario.Nome,
                usuario.UsuarioLogin,
                usuario.Setor,
                usuario.Empresa
            });
        }

    }
}
