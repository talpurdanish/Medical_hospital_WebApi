using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IProcedureManager
    {

        Task<ProcedureViewModel?> GetProcedure(int id);
        Task<IEnumerable<ProcedureViewModel>> GetProcedures(DataFilter filter);

        Task<string> Create(ProcedureViewModel viewmodel);
        Task<string> Update(ProcedureViewModel viewmodel);
        Task<bool> Delete(int id);

    }
}
