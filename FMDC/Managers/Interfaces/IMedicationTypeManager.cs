using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IMedicationTypeManager
    {
        Task<MedicationTypeViewModel?> GetMedicationType(int id);
        Task<IEnumerable<MedicationTypeViewModel>> GetMedicationTypes(DataFilter filter);

        Task<string> Create(MedicationTypeViewModel viewmodel);
        Task<string> Update(MedicationTypeViewModel viewmodel);
        Task<bool> Delete(int id);

    }
}
