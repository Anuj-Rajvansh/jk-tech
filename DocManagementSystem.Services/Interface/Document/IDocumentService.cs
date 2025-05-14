using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Interface
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document> GetByIdAsync(int id);
        Task<Document> CreateAsync(Document dto);
        Task UpdateAsync(int id, Document dto);
        Task DeleteAsync(int id);
    }
}
