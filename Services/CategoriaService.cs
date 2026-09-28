using gerenciador_de_tarefas.Data;
using gerenciador_de_tarefas.DTOs.CategoriaDTOs;
using gerenciador_de_tarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace gerenciador_de_tarefas.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(AppDbContext context, ILogger<CategoriaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CategoriaResponse> CriarCategoriaAsync(int usuarioId, CriarCategoriaRequest request)
    {
        var categoria = new Categoria
        {
            Nome = request.Nome,
            Cor = request.Cor,
            UsuarioId = usuarioId
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return new CategoriaResponse
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Cor = categoria.Cor,
            TotalTarefas = 0
        };
    }

    public async Task<List<CategoriaResponse>> ListarCategoriasAsync(int usuarioId)
    {
        var categorias = await _context.Categorias
            .Where(c => c.UsuarioId == usuarioId)
            .Include(c => c.Tarefas)
            .ToListAsync();

        return categorias.Select(c => new CategoriaResponse
        {
            Id = c.Id,
            Nome = c.Nome,
            Cor = c.Cor,
            TotalTarefas = c.Tarefas.Count(t => t.Ativa)
        }).ToList();
    }

    public async Task<bool> DeletarCategoriaAsync(int categoriaId, int usuarioId)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == categoriaId && c.UsuarioId == usuarioId);

        if (categoria == null)
            return false;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return true;
    }
}
