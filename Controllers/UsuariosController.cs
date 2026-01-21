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
        private readonly string _diretorioFotos;

        public UsuariosController(AppDbContext context)
        {
            _context = context;

            _diretorioFotos = Path.Combine(Directory.GetCurrentDirectory(), "FotosUsuarios");

            if (!Directory.Exists(_diretorioFotos))
            {
                Directory.CreateDirectory(_diretorioFotos);
            }
        }


        private async Task<string?> ProcessarFoto(string? base64Completo)
        {
            if (string.IsNullOrEmpty(base64Completo) || !base64Completo.Contains("base64"))
                return null;

            try
            {
                // Identifica a extensão (png, jpg, jpeg)
                string extensao = ".png"; 
                if (base64Completo.Contains("image/jpeg") || base64Completo.Contains("image/jpg"))
                    extensao = ".jpg";
                string base64Data = base64Completo.Contains(",") ? base64Completo.Split(',')[1] : base64Completo;
                byte[] imageBytes = Convert.FromBase64String(base64Data.Trim());
                string nomeArquivo = $"{Guid.NewGuid()}{extensao}";
                string caminhoCompleto = Path.Combine(_diretorioFotos, nomeArquivo);
                await System.IO.File.WriteAllBytesAsync(caminhoCompleto, imageBytes);
                return nomeArquivo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar imagem: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Foto))
            {
                var fotoLower = dto.Foto.ToLower();
                if (!fotoLower.Contains("image/png") &&
                    !fotoLower.Contains("image/jpeg") &&
                    !fotoLower.Contains("image/jpg"))
                {
                    return BadRequest("O sistema aceita apenas arquivos PNG, JPG ou JPEG.");
                }
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("E-mail já cadastrado.");

            if (!string.IsNullOrEmpty(dto.Cpf) && await _context.Usuarios.AnyAsync(u => u.Cpf == dto.Cpf))
                return BadRequest("CPF já cadastrado.");

            string? nomeArquivoSalvo = await ProcessarFoto(dto.Foto);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Empresa = dto.Empresa,
                Setor = dto.Setor,
                superioridade = dto.superioridade,
                Cargo = dto.Cargo,
                DataNascimento = DateUtils.Converter(dto.DataNascimento),
                DataAdmissao = DateUtils.Converter(dto.DataAdmissao),
                NivelAcesso = string.IsNullOrEmpty(dto.NivelAcesso) ? "COLABORADOR" : dto.NivelAcesso.ToUpper(),
                UsuarioLogin = dto.Usuario,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Foto = nomeArquivoSalvo,
                Cpf = dto.Cpf,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha ?? "123456")
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioCreateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            usuario.Nome = dto.Nome;
            usuario.Empresa = dto.Empresa;
            usuario.superioridade = dto.superioridade;
            usuario.Setor = dto.Setor;
            usuario.Cargo = dto.Cargo;
            usuario.DataNascimento = DateUtils.Converter(dto.DataNascimento);
            usuario.DataAdmissao = DateUtils.Converter(dto.DataAdmissao);
            usuario.UsuarioLogin = dto.Usuario;
            usuario.Telefone = dto.Telefone;
            usuario.Email = dto.Email;
            usuario.Cpf = dto.Cpf;

            if (!string.IsNullOrEmpty(dto.NivelAcesso))
            {
                usuario.NivelAcesso = dto.NivelAcesso.ToUpper(); 
            }

            if (!string.IsNullOrEmpty(dto.Foto) && dto.Foto.Contains("base64"))
            {
                if (!string.IsNullOrEmpty(usuario.Foto))
                {
                    string antigoPath = Path.Combine(_diretorioFotos, usuario.Foto);
                    if (System.IO.File.Exists(antigoPath)) System.IO.File.Delete(antigoPath);
                }
                usuario.Foto = await ProcessarFoto(dto.Foto);
            }

            if (!string.IsNullOrWhiteSpace(dto.Senha))
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok((List<Usuario>?)await _context.Usuarios.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            if (!string.IsNullOrEmpty(usuario.Foto))
            {
                string caminhoArquivo = Path.Combine(_diretorioFotos, usuario.Foto);
                if (System.IO.File.Exists(caminhoArquivo)) System.IO.File.Delete(caminhoArquivo);
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}