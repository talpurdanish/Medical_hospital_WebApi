using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IProcedureTypeManager
    {
        Task<ProcedureTypeViewModel?> GetProcedureType(int id);
        Task<IEnumerable<ProcedureTypeViewModel>> GetProcedureTypes(DataFilter filter);

        Task<string> Create(ProcedureTypeViewModel viewmodel);
        Task<string> Update(ProcedureTypeViewModel viewmodel);
        Task<bool> Delete(int id);

    }
}
