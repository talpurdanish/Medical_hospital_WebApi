using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IMedicationManager
    {

        Task<MedicationViewModel?> GetMedication(int id);
        Task<IEnumerable<MedicationViewModel>> GetMedications(DataFilter filter);

        Task<string> Create(MedicationViewModel viewmodel);
        Task<string> Update(MedicationViewModel viewmodel);
        Task<bool> Delete(int id);

    }
}
