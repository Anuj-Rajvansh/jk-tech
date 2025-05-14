using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Interface.Ingestion
{
    public interface IIngestionService
    {
        Task<bool> StartIngestionAsync(int ingestionId);

        Task<Ingestionstatus> TriggerIngestionAsync();

        Task<Ingestionstatus> GetIngestionById(int ingestionId);

    }
}
