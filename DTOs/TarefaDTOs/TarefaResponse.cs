namespace gerenciador_de_tarefas.DTOs.TarefaDTOs;

public class TarefaResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Prioridade { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string? CategoriaNome { get; set; }
}
