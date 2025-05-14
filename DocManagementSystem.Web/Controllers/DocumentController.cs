using DocManagementSystem.Services.Interface;
using DocManagementSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IDocumentService service, ILogger<DocumentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all documents");
            var docs = await _service.GetAllAsync();
            return Ok(docs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Fetching document with ID {DocumentId}", id);
            var doc = await _service.GetByIdAsync(id);
            if (doc == null)
            {
                _logger.LogWarning("Document with ID {DocumentId} not found", id);
                return NotFound();
            }

            return Ok(doc);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Document dto)
        {
            _logger.LogInformation("Creating new document");
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Document dto)
        {
            _logger.LogInformation("Updating document with ID {DocumentId}", id);
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Update failed for document ID {DocumentId}", id);
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting document with ID {DocumentId}", id);
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
