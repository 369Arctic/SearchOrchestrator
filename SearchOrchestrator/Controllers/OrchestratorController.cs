using Microsoft.AspNetCore.Mvc;
using SearchOrchestrator.Application.DTO;
using SearchOrchestrator.Application.Interfaces;

namespace SearchOrchestrator.Controllers
{

    [ApiController]
    [Route("api")]
    public class OrchestratorController : ControllerBase
    {
        private readonly IIndexingService _indexingService;

        public OrchestratorController(IIndexingService indexingService)
        {
            _indexingService = indexingService;
        }

        [HttpGet("tasks/{taskId}")]
        public async Task<IActionResult> GetStatus(Guid taskId)
        {
            var result = await _indexingService.GetStatusAsync(taskId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var result = await _indexingService.SearchAsync(query);
            return Ok(result);
        }

        [HttpPost("index")]
        public async Task <IActionResult> Start([FromBody] StartIndexingRequestDto requestDto)
        {
            var result = await _indexingService.StartAsync(requestDto);
            return Ok(result);
        }
    }
}
