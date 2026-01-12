using APIArenaAuto.Data;
using APIArenaAuto.DTOs;
using APIArenaAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/inventario")]
public class InventarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("equipamentos")]
    public async Task<IActionResult> GetEquipamentos()
    {
        var lista = await _context.Equipamentos
            .Select(e => new EquipamentoListaDTO
            {
                Id = e.Id,
                Equipamento = e.NomeEquipamento,
                Categoria = e.Categoria,
                
                QtdMarcas = _context.ModelosEquipamento.Count(m => m.EquipamentoId == e.Id),
                
                QuantidadeTotal = _context.ModelosEquipamento
                                    .Where(m => m.EquipamentoId == e.Id)
                                    .Sum(m => m.Quantidade)
            })
            .ToListAsync();

        return Ok(lista);
    }

    [HttpPost("equipamentos")]
    public async Task<IActionResult> AddEquipamento([FromBody] CriarEquipamentoDTO dto)
    {
        var equipamento = new Equipamento
        {
            NomeEquipamento = dto.NomeEquipamento,
            Categoria = dto.Categoria
        };

        _context.Equipamentos.Add(equipamento);
        await _context.SaveChangesAsync();

        return Ok(equipamento);
    }
    

    [HttpGet("equipamentos/{id}/modelos")]
    public async Task<IActionResult> GetModelos(int id)
    {
        var modelos = await _context.ModelosEquipamento
            .Where(m => m.EquipamentoId == id)
            .Select(m => new ModeloEquipamentoDTO
            {
                Id = m.Id,
                EquipamentoId = m.EquipamentoId,
                Modelo = m.Modelo,
                Quantidade = m.Quantidade
            })
            .ToListAsync();

        return Ok(modelos);
    }

    [HttpPost("modelos")]
    public async Task<IActionResult> AddModelo([FromBody] ModeloEquipamentoDTO dto)
    {
        var modelo = new ModeloEquipamento
        {
            EquipamentoId = dto.EquipamentoId,
            Modelo = dto.Modelo,
            Quantidade = dto.Quantidade
        };

        _context.ModelosEquipamento.Add(modelo);
        await _context.SaveChangesAsync();

        return Ok(modelo);
    }

    [HttpPut("modelos/{id}/quantidade")]
    public async Task<IActionResult> UpdateQuantidade(int id, [FromBody] AtualizarQuantidadeDTO dto)
    {
        var modelo = await _context.ModelosEquipamento.FindAsync(id);

        if (modelo == null)
            return NotFound("Modelo não encontrado");

        modelo.Quantidade = dto.Quantidade;
        await _context.SaveChangesAsync();

        return Ok(modelo);
    }

    [HttpDelete("modelos/{id}")]
    public async Task<IActionResult> DeleteModelo(int id)
    {
        var modelo = await _context.ModelosEquipamento.FindAsync(id);

        if (modelo == null)
            return NotFound("Modelo não encontrado");

        _context.ModelosEquipamento.Remove(modelo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}