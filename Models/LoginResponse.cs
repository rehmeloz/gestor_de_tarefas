using gerenciador_de_tarefas.DTOs.UsuarioDTOs;

namespace gerenciador_de_tarefas.Models;

public class LoginResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UsuarioDto? Usuario { get; set; }
}
