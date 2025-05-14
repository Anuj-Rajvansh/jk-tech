using DocManagementSystem.Services.Interface.Ingestion;
using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DocManagementSystem.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IngestionController : ControllerBase
    {
        private readonly IIngestionService _ingestionService;

        public IngestionController(IIngestionService ingestionService)
        {
            _ingestionService = ingestionService;
        }

        [HttpPost("trigger")]
        public async Task<IActionResult> TriggerIngestion()
        {
            var ingestion = await _ingestionService.TriggerIngestionAsync();
            
            _ = Task.Run(async () => await _ingestionService.StartIngestionAsync(ingestion.Id));

            return Ok(new { ingestion.Id, Message = "Ingestion started." });
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var ingestion = await _ingestionService.GetIngestionById(id);
            if (ingestion == null) return NotFound();

            return Ok(ingestion);
        }
    }
}
