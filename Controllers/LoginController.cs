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

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return Unauthorized("Usuário ou senha inválidos");

            return Ok(new LoginResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                NivelAcesso = usuario.NivelAcesso, 
                Setor = usuario.Setor ?? "",
                Empresa = usuario.Empresa ?? ""
            });
        }

    }
}
