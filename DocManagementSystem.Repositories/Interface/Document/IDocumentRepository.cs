using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocManagementSystem.Shared.Models;

namespace DocManagementSystem.Reposetories.Interface
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document> GetByIdAsync(int id);
        Task<Document> CreateAsync(Document document);
        Task UpdateAsync(Document document);
        Task DeleteAsync(int id);
    }
}
