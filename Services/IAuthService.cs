using gerenciador_de_tarefas.Models;

namespace gerenciador_de_tarefas.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
}
