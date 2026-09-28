using gerenciador_de_tarefas.DTOs.CategoriaDTOs;

namespace gerenciador_de_tarefas.Services;

public interface ICategoriaService
{
    Task<CategoriaResponse> CriarCategoriaAsync(int usuarioId, CriarCategoriaRequest request);
    Task<List<CategoriaResponse>> ListarCategoriasAsync(int usuarioId);
    Task<bool> DeletarCategoriaAsync(int categoriaId, int usuarioId);
}
