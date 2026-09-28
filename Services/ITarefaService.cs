using gerenciador_de_tarefas.DTOs.TarefaDTOs;

namespace gerenciador_de_tarefas.Services;

public interface ITarefaService
{
    Task<TarefaResponse> CriarTarefaAsync(int usuarioId, CriarTarefaRequest request);
    Task<List<TarefaResponse>> ListarTarefasAsync(int usuarioId, int? status = null, int? categoria = null);
    Task<TarefaResponse?> ObterTarefaAsync(int tarefaId, int usuarioId);
    Task<bool> AtualizarTarefaAsync(int tarefaId, int usuarioId, AtualizarTarefaRequest request);
    Task<bool> DeletarTarefaAsync(int tarefaId, int usuarioId);
    Task<bool> MarcarComoConcluidaAsync(int tarefaId, int usuarioId);
}
