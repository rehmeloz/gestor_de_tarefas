using gerenciador_de_tarefas.DTOs.CategoriaDTOs;
using gerenciador_de_tarefas.Models;
using gerenciador_de_tarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gerenciador_de_tarefas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaService service, ILogger<CategoriasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private int PegarUsuarioId()
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(usuarioIdClaim?.Value ?? "0");
    }


    [HttpPost]
    public async Task<ActionResult<CategoriaResponse>> CriarCategoria([FromBody] CriarCategoriaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.CriarCategoriaAsync(usuarioId, request);

            _logger.LogInformation($"Categoria criada por usuário {usuarioId}");
            return CreatedAtAction(nameof(ListarCategorias), resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar categoria: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponse>>> ListarCategorias()
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var categorias = await _service.ListarCategoriasAsync(usuarioId);

            _logger.LogInformation($"Categorias listadas para usuário {usuarioId}");
            return Ok(categorias);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao listar categorias: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCategoria(int id)
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.DeletarCategoriaAsync(id, usuarioId);

            if (!resultado)
                return NotFound(new { erro = "Categoria não encontrada" });

            _logger.LogInformation($"Categoria {id} deletada");
            return Ok(new { mensagem = "Categoria deletada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao deletar categoria: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }
}
