using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Repositories.Interface.Ingestion
{
    public interface IIngestionRepository
    {
        Task<Ingestionstatus> TriggerIngestionAsync(Ingestionstatus ingestion);

        Task<Ingestionstatus> GetIngestionById(int ingestionId);
    }
}
