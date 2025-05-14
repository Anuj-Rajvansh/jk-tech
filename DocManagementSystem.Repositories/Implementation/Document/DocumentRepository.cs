using DocManagementSystem.Reposetories.Interface;
using DocManagementSystem.Shared.Data;
using DocManagementSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Reposetories.Implementation
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(ApplicationDbContext context, ILogger<DocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            _logger.LogDebug("Document Repository: Fetching all documents from DB");
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> GetByIdAsync(int id)
        {
            _logger.LogDebug("Document Repository: Fetching document by ID {DocumentId}", id);
            return await _context.Documents.FindAsync(id);
        }

        public async Task<Document> CreateAsync(Document document)
        {
            _logger.LogInformation("Document Repository: Inserting new document");
            document.CreatedOn = DateTime.UtcNow;
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task UpdateAsync(Document document)
        {
            _logger.LogInformation("Document Repository: Updating document ID {DocumentId}", document.Id);
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Document Repository: Deleting document ID {DocumentId}", id);
            var doc = await _context.Documents.FindAsync(id);
            if (doc != null)
            {
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Document Repository: Document ID {DocumentId} not found for deletion", id);
            }
        }
    }
}
