using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using Microsoft.AspNetCore.Mvc;

namespace FMDC.Managers.Interfaces
{
    public interface IRecieptManager
    {
        Task<RecieptViewModel?> GetReciept(int id);
        Task<IEnumerable<RecieptViewModel>> GetReciepts(DataFilter filter);
        Task<IEnumerable<RecieptViewModel>> GetUnpaidReciepts();
        Task<IEnumerable<ProcedureViewModel>> GetRecieptProcedures(int id = 0);
        Task<IEnumerable<RecieptViewModel>> GetPatientReciepts(int id);
        

        Task<string> Create(RecieptViewModel viewmodel);
        Task<bool> Delete(int id);
        Task<string> UpdatePaidStatus(int id);

        Task<IncomeStatsViewModel?> GetStat(int id = -1);
        Task<RecieptViewModel?> GenerateReciept(int id);
    }
}
