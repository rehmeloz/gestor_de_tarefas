using gerenciador_de_tarefas.Enums;

namespace gerenciador_de_tarefas.Models;

public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public int? CategoriaId { get; set; }
    public int Prioridade { get; set; } = 1;
    public TarefaStatus Status { get; set; } = TarefaStatus.Pendente;
    public DateTime DataVencimento { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public bool Ativa { get; set; } = true;

    public Usuario? Usuario { get; set; }
    public Categoria? Categoria { get; set; }
}
