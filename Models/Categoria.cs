namespace gerenciador_de_tarefas.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public Usuario? Usuario { get; set; }
    public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
