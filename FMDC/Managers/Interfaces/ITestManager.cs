using Domain.Helpers;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface ITestManager
    {

        Task<TestViewModel?> GetTest(int id);
        Task<IEnumerable<TestViewModel>> GetTests(DataFilter filter);

        Task<string> Create(TestViewModel viewmodel);
        Task<string> Update(TestViewModel viewmodel);
        Task<bool> Delete(int id);

    }
}
