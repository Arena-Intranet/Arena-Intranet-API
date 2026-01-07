using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using APIArenaAuto.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: api/usuarios
        // ============================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        // ============================
        // GET: api/usuarios/{id}
        // ============================
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        // ============================
        // POST: api/usuarios
        // ============================
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email já cadastrado.");

            var cpfLimpo = StringUtils.SomenteNumeros(dto.Cpf);

            if (!string.IsNullOrEmpty(cpfLimpo) &&
                await _context.Usuarios.AnyAsync(u => u.Cpf == cpfLimpo))
                return BadRequest("CPF já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Empresa = dto.Empresa,
                Setor = dto.Setor,
                Cargo = dto.Cargo,

                DataNascimento = DateUtils.Converter(dto.DataNascimento),
                DataAdmissao = DateUtils.Converter(dto.DataAdmissao),

                UsuarioLogin = dto.Usuario,
                Telefone = StringUtils.SomenteNumeros(dto.Telefone),
                Email = dto.Email,
                Foto = dto.Foto,
                Cpf = cpfLimpo,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha ?? "123456")
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // ============================
        // PUT: api/usuarios/{id}
        // ============================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioCreateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            usuario.Nome = dto.Nome;
            usuario.Empresa = dto.Empresa;
            usuario.Setor = dto.Setor;
            usuario.Cargo = dto.Cargo;

            usuario.DataNascimento = DateUtils.Converter(dto.DataNascimento);
            usuario.DataAdmissao = DateUtils.Converter(dto.DataAdmissao);

            usuario.UsuarioLogin = dto.Usuario;
            usuario.Telefone = StringUtils.SomenteNumeros(dto.Telefone);
            usuario.Email = dto.Email;
            usuario.Foto = dto.Foto;
            usuario.Cpf = StringUtils.SomenteNumeros(dto.Cpf);

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================
        // DELETE: api/usuarios/{id}
        // ============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
