using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using FMDC.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace FMDC.Managers.Managers
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public AppointmentManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentViewModel?> GetAppointment(int id)
        {
            try
            {
                if (id <= 0)
                    return null;
                else
                {
                    var query = (from a in _context.Appointments
                                 join u in _context.Users on a.UserId equals u.UserId
                                 join p in _context.Patients on a.PatientId equals p.PatientId
                                 where a.Appointmentid == id
                                 select new AppointmentViewModel
                                 {
                                     Id = a.Appointmentid,
                                     AppointmentDate = a.AppointDate,
                                     StartTime = a.StartDtTime,
                                     AppointmentEndDate = a.AppointEndDate,
                                     EndTime = a.EndDtTime,
                                     UserId = u.UserId,
                                     PatientId = p.PatientId,
                                     PatientName = p.Name,
                                     DoctorName = u.Name
                                 });

                    return await query!.FirstOrDefaultAsync();

                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        private async Task<IEnumerable<AppointmentViewModel>> FetchAppointments()
        {
            var query = (from a in _context.Appointments
                         join u in _context.Users on a.UserId equals u.UserId
                         join p in _context.Patients on a.PatientId equals p.PatientId
                         select new AppointmentViewModel
                         {
                             Id = a.Appointmentid,
                             AppointmentDate = a.AppointDate,
                             StartTime = a.StartDtTime,
                             AppointmentEndDate = a.AppointEndDate,
                             EndTime = a.EndDtTime,
                             UserId = u.UserId,
                             PatientId = p.PatientId,
                             PatientName = p.Name,
                             DoctorName = u.Name
                         });

            var data = await query.ToListAsync();


            return data;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAppointments(DataFilter filter, int id = -1)
        {

            var appointments = await FetchAppointments();
            if (id > 0)
            {
                var doctor = await _context.Users.FindAsync(id);
                if (doctor is not null && doctor.Role == Roles.Doctor)
                    throw new FmdcException("Doctor cannot be found");
                appointments = appointments.Where(a => a.UserId == id);
            }

            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, appointments));


        }

        private static IEnumerable<AppointmentViewModel> Search(string term, IEnumerable<AppointmentViewModel> appointment)
        {
            IEnumerable<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return appointment;
            }
            else
            {
                var query = from a in appointment
                            where
                            a.PatientName!.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(term.ToLower(System.Globalization.CultureInfo.InvariantCulture)) || a.DoctorName!.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(term.ToLower(System.Globalization.CultureInfo.InvariantCulture))
                            select a;


                return query.ToList();
            }
        }

        private static IEnumerable<AppointmentViewModel> Sort(int field, int order, IEnumerable<AppointmentViewModel> appointments)
        {
            
            appointments = field switch
            {
                1 => order == 1 ? appointments.OrderBy(p => p.AppointmentDate) : appointments.OrderByDescending(p => p.AppointmentDate),
                2 => order == 1 ? appointments.OrderBy(p => p.StartTime) : appointments.OrderByDescending(p => p.StartTime),
                3 => order == 1 ? appointments.OrderBy(p => p.AppointmentEndDate) : appointments.OrderByDescending(p => p.AppointmentEndDate),
                4 => order == 1 ? appointments.OrderBy(p => p.EndTime) : appointments.OrderByDescending(p => p.EndTime),
                5 => order == 1 ? appointments.OrderBy(p => p.DoctorName) : appointments.OrderByDescending(p => p.DoctorName),
                6 => order == 1 ? appointments.OrderBy(p => p.PatientName) : appointments.OrderByDescending(p => p.PatientName),
                _ => appointments,
            };
            return appointments;
        }

        public async Task<string> Create(AddAppointmentViewModel viewModel)
        {
            var PatientId = viewModel.PatientId;
            var UserId = viewModel.UserId;
            var currentDate = DateTime.Now;

            var patientExists = await _context.Patients.AnyAsync(u => u.PatientId == PatientId);
            if (!patientExists)
                throw new FmdcException("Patient does not exists");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == UserId);
            if (!userExists)
                throw new FmdcException("User does not exists");


            Appointment model = new()
            {
                PatientId = PatientId,
                UserId = UserId,
                AppointDate = currentDate,
                StartDtTime = currentDate.Hour + ":" + currentDate.Minute
            };

            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();
            return "Appointment has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return false;
                }

                _context.Entry(appointment).State = EntityState.Deleted;
                _context.Remove(appointment);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<AppointmentStatViewModel?> GetStat(int id = -1)
        {
            var curDate = DateTime.Now;
            var selDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0, 0);
            var appointments = await FetchAppointments();

            var todayspending = (from a in appointments where !string.IsNullOrEmpty(a.StartTime) && string.IsNullOrEmpty(a.EndTime) && a.AppointmentDate.Date == curDate.Date select a.Id).Count();
            var todaystotal = (from a in appointments where a.AppointmentDate.Date == curDate.Date select a.Id).Count();
            var pending = (from a in appointments where !string.IsNullOrEmpty(a.StartTime) && string.IsNullOrEmpty(a.EndTime) select a.Id).Count();
            var total = (from a in appointments select a.Id).Count();

            if (id > 0)
            {
                var doctor = await _context.Users.FindAsync(id);
                if (doctor is not null && doctor.Role == Roles.Doctor)
                    throw new FmdcException("Doctor cannot be found");
                todayspending = (from a in appointments where a.UserId == id && a.StartTime != null && a.EndTime == null && a.AppointmentDate.Date == curDate.Date select a.Id).Count();
                todaystotal = (from a in appointments where a.UserId == id && a.AppointmentDate.Date == curDate.Date select a.Id).Count();
                pending = (from a in appointments where a.UserId == id && a.StartTime != null && a.EndTime == null select a.Id).Count();
                total = (from a in appointments where a.UserId == id select a.Id).Count();
            }
            

            return new AppointmentStatViewModel(todayspending, todaystotal, pending, total);

        }

        public async Task<IEnumerable<AppointmentViewModel>> GetPatientAppointments(int id)
        {

            if (id > 0)
            {
                throw new FmdcException("id is not valid");
            }
            var appointments = await FetchAppointments();
            appointments = appointments.Where(a => a.PatientId == id);
            return appointments;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetPending(int id = -1)
        {
            var appointments = await FetchAppointments();
            appointments = appointments.Where(a => a.AppointmentEndDate == null);
            if (id > 0)
            {
                var doctor = await _context.Users.FindAsync(id);
                if (doctor is null)
                    throw new FmdcException("User not logged in");
                appointments = appointments.Where(a => a.UserId == id);
            }
            return appointments;
        }

        public async Task<string> AddEndDate(int id, string? type = "p")
        {

            if (id <= 0)
            {
                throw new FmdcException("Id is not valid");
            }

            var currentDate = DateTime.Now;
            var model = await _context.Appointments.FirstOrDefaultAsync(a => a.PatientId == id && a.AppointEndDate == null);
            if (type == "a")
            {
                model = await _context.Appointments.FirstOrDefaultAsync(a => a.Appointmentid == id && a.AppointEndDate == null);
            }
            if (model != null)
            {
                model.AppointEndDate = currentDate;
                model.EndDtTime = currentDate.Hour + ":" + currentDate.Minute;
                _context.Entry(model).State = EntityState.Modified;
                _context.Appointments.Update(model);
                await _context.SaveChangesAsync();

                return "Appointment has been updated";
            }
            else
            {
                throw new FmdcException("Appointment could not be found");
            }
        }
    }
}
