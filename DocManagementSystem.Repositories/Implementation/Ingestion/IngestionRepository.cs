using DocManagementSystem.Repositories.Interface.Ingestion;
using DocManagementSystem.Shared.Data;
using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Repositories.Implementation.Ingestion
{
    public class IngestionRepository : IIngestionRepository
    {
        private readonly ApplicationDbContext _context;
        public IngestionRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Ingestionstatus> GetIngestionById(int ingestionId)
        {
           return _context.Ingestionstatuses.FirstOrDefault(ing => ing.Id == ingestionId);
        }


        public async Task<Ingestionstatus> TriggerIngestionAsync(Ingestionstatus ingestion)
        {
            _context.Ingestionstatuses.Add(ingestion);
            await _context.SaveChangesAsync();
            return ingestion;
        }
    }
}
