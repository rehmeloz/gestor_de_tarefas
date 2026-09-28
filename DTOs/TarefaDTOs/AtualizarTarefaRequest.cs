using gerenciador_de_tarefas.Enums;

namespace gerenciador_de_tarefas.DTOs.TarefaDTOs;

public class AtualizarTarefaRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int Prioridade { get; set; } = 1;
    public int Status { get; set; }
    public DateTime DataVencimento { get; set; }
}
