using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IProvinceManager
    {
        Task<IEnumerable<ProvinceViewModel>> GetProvinces();
    }
}
