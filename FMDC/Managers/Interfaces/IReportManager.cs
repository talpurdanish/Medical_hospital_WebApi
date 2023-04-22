using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IReportManager
    {

        Task<LabReportViewModel?> GetReport(int id);
        Task<IEnumerable<LabReportViewModel>> GetReports(DataFilter filter);
        Task<IEnumerable<LabReportViewModel>> GetPatientReports(int id);
        Task<IEnumerable<TestParameter>> GetPendingParameters(int id);
        Task<IEnumerable<LabReportViewModel>> GetPendingReports();


        Task<string> Create(LabReportViewModel viewmodel);
        Task<string> Update(LabReportViewModel viewmodel);
        Task<string> UpdateValues(AddReportValues reportValues);
        Task<bool> Delete(int id);

    }
}
