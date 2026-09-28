namespace gerenciador_de_tarefas.DTOs.TarefaDTOs;

public class CriarTarefaRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int Prioridade { get; set; } = 1;
    public DateTime DataVencimento { get; set; }
}
