using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using FMDC.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FMDC.Managers.Managers
{
    public class PrescriptionManager : IPrescriptionManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public PrescriptionManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PrescriptionViewModel?> GetPrescription(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = await (from pres in _context.Prescriptions
                               join a in _context.Appointments on pres.Appointmentid equals a.Appointmentid
                               join p in _context.Patients on a.PatientId equals p.PatientId
                               join u in _context.Users on a.UserId equals u.UserId
                               where pres.PrescriptionId == id
                               select new PrescriptionViewModel
                               {
                                   ID = pres.PrescriptionId,
                                   Appointmentid = a.Appointmentid,
                                   Doctor = u.Name,
                                   StartTime = a.StartDtTime,
                                   PatientName = p.Name,
                                   PatientNumber = p.PatientNumber,
                                   PatientId = p.PatientId,
                                   Date = a.AppointDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                   Medstrings = GenerateMedicationList(_context, pres.PrescriptionId),
                                   Tests = GenerateLabTestsList(_context, pres.PrescriptionId),
                                   Bp = pres.Bp,
                                   Bsr = pres.Bsr,
                                   Pulse = pres.Pulse,
                                   Temp = pres.Temp,
                                   Wt = pres.Wt,
                                   Ht = pres.Ht,
                               }).FirstOrDefaultAsync();

            return query;
        }

        private async Task<IEnumerable<PrescriptionViewModel>> FetchPrescriptions()
        {
            var query = await (from pres in _context.Prescriptions
                               join a in _context.Appointments on pres.Appointmentid equals a.Appointmentid
                               join p in _context.Patients on a.PatientId equals p.PatientId
                               join u in _context.Users on a.UserId equals u.UserId
                               select new PrescriptionViewModel
                               {
                                   ID = pres.PrescriptionId,
                                   Appointmentid = a.Appointmentid,
                                   Doctor = u.Name,
                                   StartTime = a.StartDtTime,
                                   PatientName = p.Name,
                                   PatientNumber = p.PatientNumber,
                                   PatientId = p.PatientId,
                                   Date = a.AppointDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                   Medstrings = GenerateMedicationList(_context, pres.PrescriptionId),
                                   Tests = GenerateLabTestsList(_context, pres.PrescriptionId),
                                   Bp = pres.Bp,
                                   Bsr = pres.Bsr,
                                   Pulse = pres.Pulse,
                                   Temp = pres.Temp,
                                   Wt = pres.Wt,
                                   Ht = pres.Ht,
                               }).ToListAsync();



            return query;
        }
        public async Task<IEnumerable<PrescriptionViewModel>> GetPrescriptions(DataFilter filter)
        {

            var Prescriptions = await FetchPrescriptions();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, Prescriptions));


        }
        private static IEnumerable<PrescriptionViewModel> Search(string term, IEnumerable<PrescriptionViewModel> Prescription)
        {
            IEnumerable<PrescriptionViewModel> Prescriptions = new List<PrescriptionViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return Prescription;
            }
            else
            {
                var query = from u in Prescription
                            where
                           u.PatientName!.Contains(term) || u.Doctor!.Contains(term)
                            select u;


                return query.ToList();
            }
        }


        private static IEnumerable<PrescriptionViewModel> Sort(int field, int order, IEnumerable<PrescriptionViewModel> list)
        {
            IEnumerable<PrescriptionViewModel> listO = new List<PrescriptionViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Doctor) : list.OrderByDescending(p => p.Doctor),
                2 => order == 1 ? list.OrderBy(p => p.PatientName) : list.OrderByDescending(p => p.PatientName),
                3 => order == 1 ? list.OrderBy(p => p.Date) : list.OrderByDescending(p => p.Date),
                4 => order == 1 ? list.OrderBy(p => p.StartTime) : list.OrderByDescending(p => p.StartTime),
                _ => list,
            };
            return listO;
        }

        public async Task<IEnumerable<PrescriptionViewModel>> GetPatientPrescriptions(int id)
        {

            if (id <= 0)
                throw new FmdcException("Id is not valid");
            var prescriptions = await FetchPrescriptions();
            prescriptions = prescriptions.Where(p => p.PatientId == id);
            return prescriptions;


        }


        public async Task<string> Create(AddPrescriptionViewModel viewmodel)
        {
            var model = new Prescription()
            {
                Appointmentid = viewmodel.AppointmentId,
                Bp = viewmodel.Bp,
                Bsr = viewmodel.Bsr,
                Pulse = viewmodel.Pulse,
                Temp = viewmodel.Temp,
                Wt = viewmodel.Wt,
                Ht = viewmodel.Ht,
                Diagnosis = viewmodel.Diagnosis,
                Remarks = viewmodel.Remarks,
            };

            _context.Prescriptions.Add(model);
            var presAdded = await _context.SaveChangesAsync();
            if (presAdded <= 0)
            {
                throw new FmdcException("Prescription could not be created");
            }
            var presMedList = GenerateMedicationList(viewmodel.Medications, model.PrescriptionId);
            _context.PrescriptionMedications.AddRange(presMedList);

            var labTestsList = GenerateLabTestsList(viewmodel.Tests, viewmodel.PatientId, viewmodel.DoctorId, model.PrescriptionId);
            _context.LabReports.AddRange(labTestsList);

            var presMedAdded = await _context.SaveChangesAsync();
            if (presMedAdded <= 0)
                throw new FmdcException("Prescription has not been created");
            return "Prescription has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var Prescription = await _context.Prescriptions.FindAsync(id);
                if (Prescription == null)
                {
                    return false;
                }

                _context.Entry(Prescription).State = EntityState.Deleted;
                _context.Prescriptions.Remove(Prescription);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<PrescriptionViewModel?> GetReportData(int id)
        {
            if (id <= 0)
            {
                throw new FmdcException("Id cannot be null");
            }
            var query = (from a in _context.Appointments
                         join p in _context.Patients on a.PatientId equals p.PatientId
                         join u in _context.Users on a.UserId equals u.UserId
                         where a.Appointmentid == id
                         select new PrescriptionViewModel()
                         {
                             Appointmentid = a.Appointmentid,
                             PatientNumber = p.PatientNumber,
                             PatientName = p.Name,
                             Date = a.AppointDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                             StartTime = a.StartDtTime,
                             PatientId = p.PatientId,
                             DoctorId = a.UserId,
                             Doctor = u.Name,


                         });
            var pModel = await query!.FirstOrDefaultAsync();
            //var model = await _context.Appointments.FirstOrDefaultAsync(p => p.Appointmentid == id);

            //PrescriptionViewModel pModel = new();
            //if (model != null)
            //{
            //    var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == model.PatientId);
            //    if (patient != null)
            //    {

            //        pModel.Appointmentid = model.Appointmentid;
            //        pModel.PatientNumber = patient.PatientNumber;
            //        pModel.PatientName = patient.Name;
            //        pModel.Date = model.AppointDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //        pModel.StartTime = model.StartDtTime;
            //        pModel.PatientId = model.PatientId;
            //        pModel.DoctorId = model.UserId;
            //    }
            //}
            return pModel;
        }
        public async Task<SlipViewModel> GeneratePrescription(int id)
        {
            SlipViewModel slipViewModel = new();
            if (id <= 0)
            {
                throw new FmdcException("Id is not valid");
            }

            var query = await (from pres in _context.Prescriptions
                               join a in _context.Appointments on pres.Appointmentid equals a.Appointmentid
                               join p in _context.Patients on a.PatientId equals p.PatientId
                               join u in _context.Users on a.UserId equals u.UserId
                               join c in _context.Cities on p.CityId equals c.CityId
                               where pres.PrescriptionId == id
                               select new SlipViewModel
                               {

                                   Doctor = u.Name,
                                   Date = a.AppointDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                   Name = p.Name,
                                   FatherName = p.FatherName ?? "",
                                   Address = p.Address ?? "",
                                   City = c.Name ?? "",
                                   BloodGroup = p.BloodGroup ?? "",
                                   Age = new Age(p.DateofBirth).AgeString,
                                   PhoneNo = p.PhoneNo ?? "",
                                   PatientNumber = p.PatientNumber,
                                   Number = pres.PrescriptionId.ToString("d8",System.Globalization.CultureInfo.InvariantCulture),
                                   Medstrings = GenerateMedicationList(_context, pres.PrescriptionId),
                                   Tests = GenerateLabTestsList(_context, pres.PrescriptionId),
                                   Diagnosis = pres.Diagnosis,
                                   Remarks = pres.Remarks,
                                   Bp = pres.Bp,
                                   Bsr = pres.Bsr,
                                   Pulse = pres.Pulse,
                                   Temp = pres.Temp,
                                   Wt = pres.Wt,
                                   Ht = pres.Ht,

                               }).FirstOrDefaultAsync();

            return query!;

        }
        private static IList<PrescriptionMedication> GenerateMedicationList(string arrayString, int pId)
        {

            arrayString = arrayString.Replace('[', ' ').Replace(']', ' ').Replace("\"", "");

            IList<PrescriptionMedication> list = new List<PrescriptionMedication>();
            if (!string.IsNullOrEmpty(arrayString))
            {
                string[] aStr = arrayString.Split(',');
                foreach (var str in aStr)
                {

                    var strArray = str.Split(':');
                    if (strArray.Length == 4)
                    {
                        list.Add(new PrescriptionMedication()
                        {
                            MedicationCode = int.Parse(strArray[0],System.Globalization.CultureInfo.InvariantCulture),
                            Quantity = double.Parse(strArray[1],System.Globalization.CultureInfo.InvariantCulture),
                            Units = strArray[2],
                            Times = int.Parse(strArray[3],System.Globalization.CultureInfo.InvariantCulture),
                            PrescriptionId = pId
                        });
                    }
                }
            }
            return list;
        }

        private static IList<PrescriptionMedicationViewModel> GenerateMedicationList(MedicalContext context, int pId)
        {

            var query = (from pm in context.PrescriptionMedications
                         join m in context.Medications on pm.MedicationCode equals m.Code
                         where pm.PrescriptionId == pId
                         select new PrescriptionMedicationViewModel()
                         {
                             Medication = m.Name,
                             Quantity = pm.Quantity,
                             Units = pm.Units,
                             Times = pm.Times
                         });


            //string.Join(
            //m.Name + " .............. ",
            //pm.Quantity + " ",
            //pm.Units + " ",
            //pm.Times + " times/day" )
            return query.ToList();
        }

        private static IList<string> GenerateLabTestsList(MedicalContext context, int pId)
        {

            var query = (from pm in context.PrescriptionMedications
                         join lr in context.LabReports on pm.PrescriptionId equals lr.PrescriptionId
                         join t in context.Tests on lr.TestId equals t.Id
                         where pm.PrescriptionId == pId
                         select t.Name);
            return query.ToList<string>();
        }

        private IList<LabReport> GenerateLabTestsList(string tests, int patientId, int doctorId, int prescriptionId)
        {
            var testIds = StringToArray(tests);
            var testList = new List<LabReport>();
            if (testIds.Length > 0)
            {
                var now = DateTime.Now;
                var delivery = now.AddDays(7);

                var nowDate = now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var nowTime = now.ToString("HH:mm", CultureInfo.InvariantCulture);

                var deliveryDate = delivery.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var deliveryTime = delivery.ToString("HH:mm", CultureInfo.InvariantCulture);

                var reportTime = TimeSpan.ParseExact(nowTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);
                var rDeliveryTime = TimeSpan.ParseExact(deliveryTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);

                var maxReportNumber = _context.LabReports.Max(l => l.ReportNumber);


                foreach (var id in testIds)
                {
                    var report = new LabReport()
                    {
                        TestId = id,
                        ReportDate = DateOnly.ParseExact(nowDate, "dd/MM/yyyy", null, DateTimeStyles.None),
                        ReportTime = reportTime,
                        ReportDeliveryDate = DateOnly.ParseExact(deliveryDate, "dd/MM/yyyy", null, DateTimeStyles.None),
                        ReportDeliveryTime = rDeliveryTime,
                        PatientId = patientId,
                        DoctorId = doctorId,
                        Note = "Lab report has been created as prescribed by Doctor via Prescription Form",
                        ReportNumber = maxReportNumber++,
                        PrescriptionId = prescriptionId

                    };
                    testList.Add(report);
                }

            }
            return testList;
        }


        private static int[] StringToArray(string arrayString)
        {
            int[] rInt;

            arrayString = arrayString.Replace('[', ' ').Replace(']', ' ');
            if (!string.IsNullOrEmpty(arrayString))
            {
                if (int.TryParse(arrayString, out int onlyInt))
                {
                    rInt = new int[] { onlyInt };
                }
                else
                {

                    string[] aStr = arrayString.Split(',');
                    rInt = new int[aStr.Length];
                    int i = 0;
                    foreach (var str in aStr)
                    {
                        rInt[i] = int.Parse(str,System.Globalization.CultureInfo.InvariantCulture);
                        i++;
                    }
                }
            }
            else
                rInt = Array.Empty<int>();
            return rInt;
        }


    }
}
