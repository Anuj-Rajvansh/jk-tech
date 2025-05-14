using DocManagementSystem.Reposetories.Interface;
using DocManagementSystem.Services.Interface;
using DocManagementSystem.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repo;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(IDocumentRepository repo, ILogger<DocumentService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<IEnumerable<Document>> GetAllAsync()
        {
            _logger.LogDebug("Service: Getting all documents");
            return _repo.GetAllAsync();
        }

        public Task<Document> GetByIdAsync(int id)
        {
            _logger.LogDebug("Service: Getting document by ID {DocumentId}", id);
            return _repo.GetByIdAsync(id);
        }

        public Task<Document> CreateAsync(Document dto)
        {
            _logger.LogInformation("Service: Creating new document");
            var doc = new Document
            {
                Name = dto.Name,
                Content = dto.Content
            };
            return _repo.CreateAsync(doc);
        }

        public async Task UpdateAsync(int id, Document dto)
        {
            _logger.LogInformation("Service: Updating document ID {DocumentId}", id);
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Service: Document ID {DocumentId} not found for update", id);
                throw new KeyNotFoundException("Document not found");
            }

            existing.Name = dto.Name;
            existing.Content = dto.Content;
            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(int id)
        {
            _logger.LogInformation("Service: Deleting document ID {DocumentId}", id);
            return _repo.DeleteAsync(id);
        }
    }
}
