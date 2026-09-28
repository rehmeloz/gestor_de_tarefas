namespace gerenciador_de_tarefas.DTOs.CategoriaDTOs;

public class CategoriaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int TotalTarefas { get; set; }
}
