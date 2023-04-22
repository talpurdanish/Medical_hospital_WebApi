using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface ICityManager
    {
        Task<IEnumerable<CityViewModel>> GetCities(int id = -1);
    }
}
