namespace gerenciador_de_tarefas.Models;

public class Usuario
{
    public int Id{ get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;

    public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
}
