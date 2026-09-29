using gerenciador_de_tarefas.DTOs.TarefaDTOs;
using gerenciador_de_tarefas.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gerenciador_de_tarefas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _service;
    private readonly ILogger<TarefasController> _logger;

    public TarefasController(ITarefaService service, ILogger<TarefasController> logger)
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
    public async Task<ActionResult<TarefaResponse>> CriarTarefa([FromBody] CriarTarefaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.CriarTarefaAsync(usuarioId, request);

            _logger.LogInformation($"Tarefa criada por usuário {usuarioId}");
            return CreatedAtAction(nameof(ObterTarefa), new { id = resultado.Id }, resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar tarefa: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<TarefaResponse>>> ListarTarefas([FromQuery] int? status = null, [FromQuery] int? categoria = null)
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var tarefas = await _service.ListarTarefasAsync(usuarioId, status, categoria);

            _logger.LogInformation($"Tarefas listadas para usuário {usuarioId}");
            return Ok(tarefas);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao listar tarefas: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TarefaResponse>> ObterTarefa(int id)
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var tarefa = await _service.ObterTarefaAsync(id, usuarioId);

            if (tarefa == null)
                return NotFound(new { erro = "Tarefa não encontrada" });

            return Ok(tarefa);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao obter tarefa: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarTarefa(int id, [FromBody] AtualizarTarefaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.AtualizarTarefaAsync(id, usuarioId, request);

            if (!resultado)
                return NotFound(new { erro = "Tarefa não encontrada" });

            _logger.LogInformation($"Tarefa {id} atualizada");
            return Ok(new { mensagem = "Tarefa atualizada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao atualizar tarefa: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarTarefa(int id)
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.DeletarTarefaAsync(id, usuarioId);

            if (!resultado)
                return NotFound(new { erro = "Tarefa não encontrada" });

            _logger.LogInformation($"Tarefa {id} deletada");
            return Ok(new { mensagem = "Tarefa deletada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao deletar tarefa: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> MarcarComoConcluida(int id)
    {
        try
        {
            var usuarioId = PegarUsuarioId();
            var resultado = await _service.MarcarComoConcluidaAsync(id, usuarioId);

            if (!resultado)
                return NotFound(new { erro = "Tarefa não encontrada" });

            _logger.LogInformation($"Tarefa {id} marcada como concluída");
            return Ok(new { mensagem = "Tarefa marcada como concluída" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao marcar tarefa como concluída: {ex.Message}");
            return BadRequest(new { erro = ex.Message });
        }
    }
}

