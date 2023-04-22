using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IPrescriptionManager
    {

        Task<PrescriptionViewModel?> GetPrescription(int id);
        Task<IEnumerable<PrescriptionViewModel>> GetPrescriptions(DataFilter filter);
        Task<PrescriptionViewModel?> GetReportData(int id);
        Task<IEnumerable<PrescriptionViewModel>> GetPatientPrescriptions(int id);
        Task<string> Create(AddPrescriptionViewModel viewmodel);
        
        Task<bool> Delete(int id);

        Task<SlipViewModel> GeneratePrescription(int id);
    }
}
