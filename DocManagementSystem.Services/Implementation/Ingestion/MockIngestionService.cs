using DocManagementSystem.Repositories.Interface.Ingestion;
using DocManagementSystem.Services.Interface.Ingestion;
using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Implementation.Ingestion
{
    public class MockIngestionService : IIngestionService
    {
        private readonly IIngestionRepository _ingestionRepository;

        public MockIngestionService(IIngestionRepository ingestionRepository)
        {
            _ingestionRepository = ingestionRepository;
        }

        public async Task<Ingestionstatus> GetIngestionById(int ingestionId)
        {
            return await _ingestionRepository.GetIngestionById(ingestionId);
        }


        public async Task<bool> StartIngestionAsync(int ingestionId)
        {
            var ingestion = await _ingestionRepository.GetIngestionById(ingestionId);
            if (ingestion == null) return false;

            ingestion.Status = IngestionStatusEnum.Running.ToString();
            await _ingestionRepository.TriggerIngestionAsync(ingestion);

            await Task.Delay(2000); 
            var random = new Random();
            var success = random.Next(0, 2) == 1;

            ingestion.Status = success ? IngestionStatusEnum.Completed.ToString() : IngestionStatusEnum.Failed.ToString();
            ingestion.ErrorMessage = success ? null : "Mock ingestion failed.";
            await _ingestionRepository.TriggerIngestionAsync(ingestion);

            return success;
        }

        public async Task<Ingestionstatus> TriggerIngestionAsync()
        {
            var ingestion = new Ingestionstatus
            {
                TriggeredAt = DateTime.UtcNow,
                Status = IngestionStatusEnum.Pending.ToString(),
            };

           return await _ingestionRepository.TriggerIngestionAsync(ingestion);
        }
    }
}
