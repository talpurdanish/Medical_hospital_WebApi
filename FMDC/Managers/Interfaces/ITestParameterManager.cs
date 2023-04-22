using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface ITestParameterManager
    {

        Task<TestParameterViewModel?> GetTestParameter(int id);
        Task<IEnumerable<TestParameterViewModel>> GetTestParameters(DataFilter filter);
        Task<IEnumerable<TestParameterViewModel>> GetTestParameters(int testId);

        Task<string> Create(TestParameterViewModel viewmodel);
        Task<string> Update(TestParameterViewModel viewmodel);
        
        Task<bool> Delete(int id);

    }
}
