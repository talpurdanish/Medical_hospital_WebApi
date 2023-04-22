using Domain.Helpers;

using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface IAppointmentManager
    {
        Task<AppointmentViewModel?> GetAppointment(int id);
        Task<IEnumerable<AppointmentViewModel>> GetAppointments(DataFilter filter, int id = -1);
        Task<IEnumerable<AppointmentViewModel>> GetPatientAppointments(int id);
        Task<IEnumerable<AppointmentViewModel>> GetPending(int id = -1);

        Task<string> Create(AddAppointmentViewModel viewModel);
        Task<bool> Delete(int id);
        Task<string> AddEndDate(int id, string? type = "p");

        Task<AppointmentStatViewModel?> GetStat(int id = -1);



    }
}
