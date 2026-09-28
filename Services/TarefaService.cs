using gerenciador_de_tarefas.Data;
using gerenciador_de_tarefas.DTOs.TarefaDTOs;
using gerenciador_de_tarefas.Enums;
using gerenciador_de_tarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace gerenciador_de_tarefas.Services;

public class TarefaService : ITarefaService
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public TarefaService(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TarefaResponse> CriarTarefaAsync(int usuarioId, CriarTarefaRequest request)
    {
        var tarefa = new Tarefa
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            UsuarioId = usuarioId,
            CategoriaId = request.CategoriaId,
            Prioridade = request.Prioridade,
            DataVencimento = request.DataVencimento,
            DataCriacao = DateTime.Now
        };

        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Tarefa criada: {tarefa.Id} para usuário {usuarioId}!");

        return MapearParaResponse(tarefa);
    }

    public async Task<List<TarefaResponse>> ListarTarefasAsync(int usuarioId, int? status = null, int? categoria = null)
    {
        var query = _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId && t.Ativa)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(t => (int)t.Status == status.Value);

        if (categoria.HasValue)
            query = query.Where(t => t.CategoriaId == categoria.Value);

        var tarefas = await query
            .Include(t => t.Categoria)
            .OrderByDescending(t => t.Prioridade)
            .ThenBy(t => t.DataVencimento)
            .ToListAsync();

        return tarefas.Select(MapearParaResponse).ToList();
    }

    public async Task<TarefaResponse?> ObterTarefaAsync(int tarefaId, int usuarioId)
    {
        var tarefa = await _context.Tarefas.Include(t => t.Categoria)
                .FirstOrDefaultAsync(t => t.Id == tarefaId && t.UsuarioId == usuarioId);

        return tarefa == null ? null : MapearParaResponse(tarefa);
    }

    public async Task<bool> AtualizarTarefaAsync(int tarefaId, int usuarioId, AtualizarTarefaRequest request)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == tarefaId && t.UsuarioId == usuarioId);

        if (tarefa == null)
            return false;

        tarefa.Titulo = request.Titulo;
        tarefa.Descricao = request.Descricao;
        tarefa.CategoriaId = request.CategoriaId;
        tarefa.Prioridade = request.Prioridade;
        tarefa.Status = (TarefaStatus)request.Status;
        tarefa.DataVencimento = request.DataVencimento;

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Tarefa {tarefaId} atualizada!");

        return true;
    }

    public async Task<bool> DeletarTarefaAsync(int tarefaId, int usuarioId)
    {
        var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t => t.Id == tarefaId && t.UsuarioId == usuarioId);

        if (tarefa == null)
            return false;

        tarefa.Ativa = false;
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Tarefa {tarefaId} deletada!");

        return true;
    }

    public async Task<bool> MarcarComoConcluidaAsync(int tarefaId, int usuarioId)
    {
        var tarefa = await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == tarefaId && t.UsuarioId == usuarioId);

        if (tarefa == null)
            return false;

        tarefa.Status = TarefaStatus.Concluida;
        tarefa.DataConclusao = DateTime.Now;
        await _context.SaveChangesAsync();

        return true;
    }

    private TarefaResponse MapearParaResponse(Tarefa tarefa)
    {
        return new TarefaResponse
        {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Status = tarefa.Status.ToString(),
            Prioridade = tarefa.Prioridade,
            DataVencimento = tarefa.DataVencimento,
            DataCriacao = tarefa.DataCriacao,
            DataConclusao = tarefa.DataConclusao,
            CategoriaNome = tarefa.Categoria?.Nome
        };
    }
}
