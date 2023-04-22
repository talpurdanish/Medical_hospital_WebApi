using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using FMDC.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SkiaSharp;
using System.Collections.Generic;

namespace FMDC.Managers.Managers
{
    public class RecieptManager : IRecieptManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public RecieptManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<RecieptViewModel?> GetReciept(int id)
        {

            if (id <= 0)
                throw new FmdcException("Id is not valid");
            else
            {
                var currentDate = DateTime.Now;
                var query = (from appoint in _context.Appointments
                             join patient in _context.Patients on appoint.PatientId equals patient.PatientId
                             join user in _context.Users on appoint.UserId equals user.UserId
                             where appoint.PatientId == id && appoint.RecieptId == null
                             select new RecieptViewModel
                             {
                                 PatientId = patient.PatientId,
                                 PatientName = patient.Name,
                                 PatientNumber = patient.PatientNumber,
                                 Date = currentDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                 Time = currentDate.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                                 DoctorId = user.UserId,
                                 Doctor = user.Name,
                                 Appointment = appoint.AppointDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture) + " " + appoint.StartDtTime,
                                 AppointmentId = appoint.Appointmentid
                             });

                return await query!.FirstOrDefaultAsync();

            }

        }
        private async Task<IEnumerable<RecieptViewModel>> FetchReciepts()
        {
            var query = (from reciept in _context.Reciepts
                         join authUser in _context.Users on reciept.AuthorizedById equals authUser.UserId into RecieptAuthGroup
                         from t1 in RecieptAuthGroup.ToList().DefaultIfEmpty()
                         join patient in _context.Patients on reciept.PatientId equals patient.PatientId
                         join user in _context.Users on reciept.UserId equals user.UserId
                         join appoint in _context.Appointments on reciept.AppointmentId equals appoint.Appointmentid
                         select new RecieptViewModel
                         {
                             PatientName = patient.Name,
                             PatientNumber = patient.PatientNumber,
                             Date = reciept.RecieptDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture),
                             Time = reciept.RecieptTime,
                             Doctor = user.Name,
                             Discount = reciept.RecieptDiscount,
                             Appointment = appoint.AppointDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture) + " " + appoint.StartDtTime,
                             ID = reciept.RecieptId,
                             Total = reciept.Total,
                             GrandTotal = reciept.GrandTotal,
                             Paid = reciept.Paid,
                             AuthorizedById = t1 == null ? 0 : t1.UserId,
                             AuthorizedBy = t1 == null ? "" : t1.Name,
                             DoctorId = user.UserId,
                             PatientId = patient.PatientId
                         });

            var data = await query.ToListAsync();

            foreach (var reciept in data)
            {
                reciept.Procedures = await (from rp in _context.RecieptProcedures
                                            join p in _context.Procedures on rp.ProcedureId equals p.ProcedureID
                                            where rp.RecieptId == reciept.ID
                                            select new ProcedureViewModel()
                                            {
                                                Name = p.ProcedureName,
                                                ID = p.ProcedureID,
                                                TypeID = p.ProcedureTypeID,
                                                Cost = p.ProcedureCost
                                            }).ToListAsync();
            }


            return data;
        }



        public async Task<IEnumerable<RecieptViewModel>> GetReciepts(DataFilter filter)
        {

            var Reciepts = await FetchReciepts();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, Reciepts));


        }
        private static IEnumerable<RecieptViewModel> Search(string term, IEnumerable<RecieptViewModel> Reciept)
        {
            IEnumerable<RecieptViewModel> Reciepts = new List<RecieptViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return Reciept;
            }
            else
            {
                var query = from u in Reciept
                            where
                           u.PatientName!.Contains(term) || u.Doctor!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<RecieptViewModel> Sort(int field, int order, IEnumerable<RecieptViewModel> list)
        {
            IEnumerable<RecieptViewModel> listO = new List<RecieptViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.PatientName) : list.OrderByDescending(p => p.PatientName),
                2 => order == 1 ? list.OrderBy(p => p.Doctor) : list.OrderByDescending(p => p.Doctor),
                3 => order == 1 ? list.OrderBy(p => p.Date) : list.OrderByDescending(p => p.Date),
                4 => order == 1 ? list.OrderBy(p => p.GrandTotal) : list.OrderByDescending(p => p.GrandTotal),
                _ => list,
            };
            return listO;
        }
        public async Task<IEnumerable<RecieptViewModel>> GetUnpaidReciepts()
        {
           var reciepts = await FetchReciepts();
            reciepts= reciepts.Where(r=>r.Paid==false);


            return reciepts;
        }
        public async Task<IEnumerable<ProcedureViewModel>> GetRecieptProcedures(int id = 0)
        {
            if (id <= 0)
            {
                var procedures = await (from rp in _context.RecieptProcedures
                                        join p in _context.Procedures on rp.ProcedureId equals p.ProcedureID
                                        where rp.RecieptId == id
                                        select new ProcedureViewModel()
                                        {
                                            Name = p.ProcedureName,
                                            Cost = p.ProcedureCost,
                                            ID = p.ProcedureID
                                        }).ToListAsync();
                return procedures;
            }
            else
            {
                throw new FmdcException("Id is not valid");
            }
        }
        public async Task<IEnumerable<RecieptViewModel>> GetPatientReciepts(int id)
        {
            
            var reciepts = await FetchReciepts();
            reciepts= reciepts.Where(r=>r.PatientId==id);


            return reciepts;
        }

        public async Task<string> Create(RecieptViewModel viewmodel)
        {
            //Reciept reciept = RecieptViewModel.GenerateModel(viewmodel);
            var reciept = _mapper.Map<Reciept>(viewmodel);
            _context.Reciepts.Add(reciept);
            var recieptSaved = await _context.SaveChangesAsync();
            if (recieptSaved > 0)
            {
                var rpList = new List<RecieptProcedure>();
                foreach (var p in viewmodel.Procedures)
                {
                    var rp = new RecieptProcedure()
                    {
                        ProcedureId = p.ID,
                        RecieptId = reciept.RecieptId

                    };

                    rpList.Add(rp);
                    //_context.RecieptProcedures.Add(rp);
                }
                await _context.RecieptProcedures.AddRangeAsync(rpList);
                var appointment = await _context.Appointments.Where(a => a.Appointmentid == reciept.AppointmentId).FirstOrDefaultAsync();
                appointment!.RecieptId = reciept.RecieptId;
                _context.Entry(appointment).State = EntityState.Modified;
                _context.Appointments.Update(appointment);

                var postRecieptSaved = await _context.SaveChangesAsync();

                return postRecieptSaved > 0 ? "Reciept has been created" : "Reciept has not been created";
            }

            return "Reciept has not been created";
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var Reciept = await _context.Reciepts.FindAsync(id);
                if (Reciept == null)
                {
                    return false;
                }

                _context.Entry(Reciept).State = EntityState.Deleted;
                _context.Remove(Reciept);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<string> UpdatePaidStatus(int id)
        {
            if (id <= 0)
            {
                throw new FmdcException("Id is not valid");
            }
            var reciept = await _context.Reciepts.FindAsync(id);
            if (reciept == null)
            {
                throw new FmdcException("Reciept could not be found");
            }
            reciept.Paid = true;
            _context.Entry(reciept).State = EntityState.Modified;
            _context.Reciepts.Update(reciept);
            await _context.SaveChangesAsync();
            return "Reciept has been paid";

        }

        public async Task<IncomeStatsViewModel?> GetStat(int id = -1)
        {
            var curDate = DateTime.Now;
            var selDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0, 0);


            var tquery = (from a in _context.Reciepts where a.RecieptDate == selDate select a.GrandTotal);
            var query = (from a in _context.Reciepts select a.GrandTotal);

            var dates = _context.Reciepts.Select(r => r.RecieptDate.ToString("dd/MM", System.Globalization.CultureInfo.InvariantCulture)).Distinct();
            var result = _context.Reciepts.GroupBy(r => r.RecieptDate.Date).Select(cl => cl.Sum(c => c.GrandTotal));


            if (id > 0)
            {
                var doctor = await _context.Users.FindAsync(id);
                if (doctor is not null && doctor.Role == Roles.Doctor)
                    throw new FmdcException("Doctor cannot be found");
                tquery = (from a in _context.Reciepts where a.UserId == doctor!.UserId && a.RecieptDate == selDate select a.GrandTotal);
                query = (from a in _context.Reciepts where a.UserId == doctor!.UserId select a.GrandTotal);

                dates = _context.Reciepts.Where(r => r.UserId == doctor!.UserId).Select(r => r.RecieptDate.ToString("dd/MM", System.Globalization.CultureInfo.InvariantCulture)).Distinct();
                result = _context.Reciepts.Where(r => r.UserId == doctor!.UserId).GroupBy(r => r.RecieptDate.Date).Select(cl => cl.Sum(c => c.GrandTotal));

            }
            

            var totalsList = await query.ToListAsync();
            var todayList = await tquery.ToListAsync();
            var datesList = await dates.ToListAsync();
            var resultList = await result.ToListAsync();
            var total = query.Sum();
            var todays = tquery.Sum();

            return new IncomeStatsViewModel()
            {
                Todays = todays,
                Total = total,
                Labels = datesList,
                Data = resultList
            };


        }
        public async Task<RecieptViewModel?> GenerateReciept(int id)
        {

            if (id <= 0)
            {
                throw new FmdcException("Id is not valid");
            }

            var query = (from reciept in _context.Reciepts
                         join authUser in _context.Users on reciept.AuthorizedById equals authUser.UserId into RecieptAuthGroup
                         from t1 in RecieptAuthGroup.ToList().DefaultIfEmpty()
                         join patient in _context.Patients on reciept.PatientId equals patient.PatientId
                         join user in _context.Users on reciept.UserId equals user.UserId
                         join appoint in _context.Appointments on reciept.AppointmentId equals appoint.Appointmentid
                         where reciept.RecieptId == id
                         select new RecieptViewModel
                         {
                             PatientName = patient.Name,
                             PatientNumber = patient.PatientNumber,
                             Date = reciept.RecieptDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture),
                             Time = reciept.RecieptTime,
                             Doctor = user.Name,
                             Discount = reciept.RecieptDiscount,
                             Appointment = appoint.AppointDate.ToString("dd MMM, yyyy", System.Globalization.CultureInfo.InvariantCulture) + " " + appoint.StartDtTime,
                             ID = reciept.RecieptId,
                             RecieptNumber = reciept.RecieptId.ToString("D8", System.Globalization.CultureInfo.InvariantCulture),
                             Paid = reciept.Paid,
                             AuthorizedById = reciept.AuthorizedById,
                             AuthorizedBy = t1 != null ? t1.Name : "",
                             Total = reciept.Total,
                             GrandTotal = reciept.GrandTotal
                         });
            RecieptViewModel? viewModel = await query!.FirstOrDefaultAsync();

            var procedures = await (from rp in _context.RecieptProcedures
                                    join p in _context.Procedures on rp.ProcedureId equals p.ProcedureID
                                    where rp.RecieptId == id
                                    select new ProcedureViewModel()
                                    {
                                        Name = p.ProcedureName,
                                        Cost = p.ProcedureCost,
                                        ID = p.ProcedureID
                                    }).ToListAsync();
            if (procedures.Count > 0)
                viewModel!.Procedures = procedures;
            return viewModel;

        }


    }
}
