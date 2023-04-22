using Domain.Helpers;

using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IPatientManager
    {
        Task<PatientViewModel?> GetPatient(int id);
        Task<IEnumerable<PatientViewModel>> GetPatients(DataFilter filter);

        Task<string> Create(PatientViewModel viewmodel);
        Task<string> Update(PatientViewModel viewmodel);
        Task<bool> Delete(int id);

        Task<bool> CheckDuplicate(DuplicateType type, string value, int id = -1);

        Task<PatientStatViewModel?> GetStat();
        Task<PatientButtons> GetButtons(int id);

        Task<SlipViewModel> GenerateSlip(int id);
    }
}
