using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {

        private readonly IAppointmentManager _manager;

        public AppointmentsController(IAppointmentManager Manager)
        {
            _manager = Manager;

        }
        // GET: api/<AppointmentsController>
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var appointments = await _manager.GetAppointments(filter);

                return FmdcResult.Success(appointments, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Appointments could not be fetched", 500);
            }
        }

        // GET api/<AppointmentsController>/5
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("AppointmentId is not valid", 500);
                var appointment = await _manager.GetAppointment(id);
                return FmdcResult.Success("", appointment, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Appointment does not exists", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetPatientAppointments(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("Patient Id is not valid", 500);
                var appointment = await _manager.GetPatientAppointments(id);
                return FmdcResult.Success("", appointment, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Appointment does not exists", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]")]
        public async Task<JsonResult> GetPending()
        {
            try
            {
                var user = GetCurrentUser();
                var id = user is not null && user.Role != "Administrator" ? user.id : -1;
                
                var appointment = await _manager.GetPending(id);
                return FmdcResult.Success("", appointment, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Appointment does not exists", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff)]
        // POST api/<AppointmentsController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] AddAppointmentViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);

                return FmdcResult.Success("Appointment has been created", null, 200);

            }
            catch (FmdcException ae)
            {

                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }


        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut("[action]")]
        public async Task<JsonResult> AddEndDate([FromBody] UserValue input)
        {
            try
            {
                var id = input.id;
                var result = await _manager.AddEndDate(id, input.value);
                return FmdcResult.Success("Appointment has been updated", null, 200);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

        // DELETE api/<AppointmentsController>/5
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("AppointmentId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("Appointment has been deleted", null, 200);
                else
                    return FmdcResult.Error("Appointment has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

         [HttpGet("[action]")]
        public async Task<JsonResult> GetStats()
        {
            try
            {
                var id = -1;
                var doctor = GetCurrentUser();
                if (doctor is not null && doctor.Role == Roles.Doctor)
                    id = doctor.id;
                var stats = await _manager.GetStat(id);
                return FmdcResult.Success("", stats, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("No Stats Available", 500);
            }
        }
        private UserViewModel? GetCurrentUser()
        {
            return HttpContext.Items["User"] as UserViewModel;

        }
    }
}
