using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using FMDC.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FMDC.Managers.Managers
{
    public class PatientManager : IPatientManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public PatientManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PatientViewModel?> GetPatient(int id)
        {
            try
            {
                if (id <= 0)
                    return null;
                else
                {
                    var query = (from patient in _context.Patients
                                 join city in _context.Cities on patient.CityId equals city.CityId
                                 join province in _context.Provinces on city.ProvinceId equals province.ProvinceId
                                 where patient.PatientId == id
                                 select new PatientViewModel
                                 {
                                     Name = patient.Name,
                                     Id = patient.PatientId,
                                     Created = patient.Created,
                                     Address = patient.Address,
                                     DateofBirth = patient.DateofBirth,
                                     PhoneNo = patient.PhoneNo,
                                     Gender = patient.Gender != null ? patient.Gender : "0",
                                     CNIC = patient.CNIC,

                                     Picture = patient.Picture != null ? String.Format(System.Globalization.CultureInfo.InvariantCulture,"data:image/png;base64,{0}", Convert.ToBase64String(patient.Picture!)) : "",
                                     City = city.Name,
                                     Province = province.Name,
                                     PatientNumber = patient.PatientNumber,
                                     CityId = patient.CityId,

                                     PhoneType = patient.PhoneType,
                                 });

                    return await query!.FirstOrDefaultAsync();

                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        private async Task<IEnumerable<PatientViewModel>> FetchPatients()
        {
            var query = (from patient in _context.Patients
                         join city in _context.Cities on patient.CityId equals city.CityId
                         join province in _context.Provinces on city.ProvinceId equals province.ProvinceId
                         select new PatientViewModel
                         {
                             Name = patient.Name,
                             Id = patient.PatientId,
                             Created = patient.Created,
                             Address = patient.Address,
                             DateofBirth = patient.DateofBirth,
                             PhoneNo = patient.PhoneNo,
                             Gender = patient.Gender != null ? patient.Gender : "0",
                             CNIC = patient.CNIC,

                             Picture = patient.Picture != null ? String.Format(System.Globalization.CultureInfo.InvariantCulture,"data:image/png;base64,{0}", Convert.ToBase64String(patient.Picture!)) : "",
                             City = city.Name,
                             Province = province.Name,
                             PatientNumber = patient.PatientNumber,
                             CityId = patient.CityId,
                             PhoneType = patient.PhoneType,

                         });

            var data = await query.ToListAsync();


            return data;
        }

        public async Task<IEnumerable<PatientViewModel>> GetPatients(DataFilter filter)
        {

            var patients = await FetchPatients();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, patients));


        }

        private static IEnumerable<PatientViewModel> Search(string term, IEnumerable<PatientViewModel> patient)
        {
            IEnumerable<PatientViewModel> patients = new List<PatientViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return patient;
            }
            else
            {
                var query = from u in patient
                            where
                            u.Name!.Contains(term) || u.PatientNumber!.Contains(term) ||
                            u.CNIC!.Contains(term) || u.City!.Contains(term) || u.PhoneNo!.Contains(term)
                            select u;


                return query.ToList();
            }
        }

        private static IEnumerable<PatientViewModel> Sort(int field, int order, IEnumerable<PatientViewModel> patient)
        {
            IEnumerable<PatientViewModel> patients = new List<PatientViewModel>();
            patients = field switch
            {
                1 => order == 1 ? patient.OrderBy(p => p.Name) : patient.OrderByDescending(p => p.Name),
                2 => order == 1 ? patient.OrderBy(p => p.PatientNumber) : patient.OrderByDescending(p => p.PatientNumber),
                3 => order == 1 ? patient.OrderBy(p => p.CNIC) : patient.OrderByDescending(p => p.CNIC),

                4 => order == 1 ? patient.OrderBy(p => p.City) : patient.OrderByDescending(p => p.City),
                _ => patient,
            };
            return patients;
        }

        public async Task<string> Create(PatientViewModel viewmodel)
        {

            var patient = await _context.Patients.FirstOrDefaultAsync(u => u.PatientId == viewmodel.Id);
            if (patient == null)
                throw new FmdcException("Patient does not exists");

            var pmdcExists = await _context.Patients.AnyAsync(u => u.PatientNumber == viewmodel.PatientNumber);
            if (pmdcExists)
                throw new FmdcException("PatientNumber already exists");


            var cnicExists = await _context.Patients.AnyAsync(u => u.CNIC == viewmodel.CNIC);
            if (cnicExists)
                throw new FmdcException("CNIC already exists");



            var model = _mapper.Map<Patient>(viewmodel);
            var maxMrNoQuery = _context.Patients.Max(p => p.MRNoID);
            var previousId = maxMrNoQuery != null ? maxMrNoQuery.Value : 1;
            model.PatientNumber = ComputePatientNumber(previousId);

            if (!string.IsNullOrEmpty(viewmodel.Picture))
            {
                var pictureString = viewmodel.Picture.Replace(@"\/", "/");
                try
                {
                    string converted = viewmodel.Picture.Replace('-', '+');
                    converted = converted.Replace('_', '/');
                    model.Picture = Convert.FromBase64String(converted);
                }
                catch (Exception)
                {
                    model.Picture = Convert.FromBase64String(pictureString);

                }
            }

            _context.Patients.Add(model);
            await _context.SaveChangesAsync();
            return "Patient has been created";

        }

        private static string ComputePatientNumber(int previousId)
        {
            return DateTime.Now.ToString("MM", System.Globalization.CultureInfo.InvariantCulture)
                + "\\" + DateTime.Now.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture)
                + "-100" + (previousId + 1);


        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return false;
                }

                _context.Entry(patient).State = EntityState.Deleted;
                _context.Remove(patient);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<string> Update(PatientViewModel viewmodel)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(u => u.PatientId == viewmodel.Id);
            if (patient == null)
                throw new FmdcException("Patient does not exists");

            var cnicExists = await _context.Patients.AnyAsync(u => u.CNIC == viewmodel.CNIC && u.PatientId != viewmodel.Id);
            if (cnicExists)
                throw new FmdcException("CNIC already exists");

            patient.Name = viewmodel.Name;
            patient.CNIC = viewmodel.CNIC;
            patient.Address = viewmodel.Address;
            patient.DateofBirth = viewmodel.DateofBirth;
            patient.FatherName = string.IsNullOrEmpty(viewmodel.FatherName) ? "" : viewmodel.FatherName;
            patient.PhoneNo = viewmodel.PhoneNo;
            patient.PhoneType = viewmodel.PhoneType;
            patient.BloodGroup = viewmodel.BloodGroup;
            patient.Gender = viewmodel.Gender;
            patient.CityId = viewmodel.CityId;
            if (!string.IsNullOrEmpty(viewmodel.Picture))
            {
                var pictureString = viewmodel.Picture.Replace(@"\/", "/");
                try
                {
                    string converted = viewmodel.Picture.Replace('-', '+');
                    converted = converted.Replace('_', '/');
                    patient.Picture = Convert.FromBase64String(converted);
                }
                catch (Exception)
                {
                    patient.Picture = Convert.FromBase64String(pictureString);

                }
            }
            _context.Entry(patient).State = EntityState.Modified;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return "Patient has been updated";

        }

        public async Task<bool> CheckDuplicate(DuplicateType type, string value, int id = -1)
        {
            bool patient = false;

            switch (type)
            {
                case DuplicateType.Pmdcno:
                    patient = id == -1 ? await _context.Patients.AnyAsync(u => u.PatientNumber == value) : await _context.Patients.AnyAsync(u => u.PatientNumber == value && u.PatientId != id);
                    break;
                case DuplicateType.Cnic:
                    patient = id == -1 ? await _context.Patients.AnyAsync(u => u.CNIC == value) : await _context.Patients.AnyAsync(u => u.CNIC == value && u.PatientId != id);
                    break;

            }

            return patient;

        }

        public async Task<PatientStatViewModel?> GetStat()
        {
            var query = from patient in _context.Patients.GroupBy(p => p.Gender)
                        select new
                        {
                            count = patient.Count(),
                            gender = patient.First().Gender
                        };

            var list = await query.ToListAsync();

            var males = await query.Where(q => q.gender == "0").Select(query => query.count).FirstOrDefaultAsync();
            var females = await query.Where(q => q.gender == "1").Select(query => query.count).FirstOrDefaultAsync();
            var others = await query.Where(q => q.gender == "2").Select(query => query.count).FirstOrDefaultAsync();
            var patients = males + females + others;

            return new PatientStatViewModel(patients, males, females, others);

        }

        public async Task<PatientButtons> GetButtons(int id)
        {
            var StartVisible = true;
            var EndVisible = false;
            var RecieptVisible = false;
            var Title = "";
            var PatientId = 0;
            var aId = 0;
            var PrescriptionVisible = false;

            if (id > 0)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (patient != null)
                {
                    var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.PatientId == id && a.RecieptId == null);

                    if (appointment != null)
                    {
                        var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Appointmentid == appointment.Appointmentid);
                        if (appointment.AppointEndDate == null)
                        {
                            StartVisible = false;
                            EndVisible = true;
                            RecieptVisible = false;
                            PrescriptionVisible = true;
                        }
                        else
                        {
                            StartVisible = false;
                            EndVisible = false;
                            RecieptVisible = true;
                        }
                        aId = appointment.Appointmentid;
                    }
                    var split = patient.PatientNumber.Split('\\');
                    if (split.Length > 0)
                        Title = patient.Name + " - " + split[0] + "/" + split[1];
                    PatientId = id;
                }

            }
            var pb = new PatientButtons
            {
                PatientId = PatientId,
                Title = Title,
                AId = aId,
                StartVisible = StartVisible,
                EndVisible = EndVisible,
                RecieptVisible = RecieptVisible,
                PrescriptionVisible = PrescriptionVisible
            };
            return pb;
        }

        public async Task<SlipViewModel> GenerateSlip(int id)
        {
            SlipViewModel slipViewModel = new();
            if (id <= 0)
            {
                throw new FmdcException("Id is not valid");
            }

            var query = (from p in _context.Patients
                         join c in _context.Cities on p.CityId equals c.CityId
                         where p.PatientId == id
                         select new PatientViewModel
                         {
                             Id = p.PatientId,
                             PatientNumber = p.PatientNumber,
                             Name = p.Name,
                             City = c.Name,
                             Address = p.Address,
                             DateofBirth = p.DateofBirth,
                             FatherName = p.FatherName,
                             PhoneNo = p.PhoneNo,
                             BloodGroup = string.IsNullOrEmpty(p.BloodGroup) ? "" : p.BloodGroup,
                             MrNo = p.MRNoID == null ? 0 : (int)p.MRNoID

                         });

            var patient = await query!.FirstOrDefaultAsync();
            if (patient != null)
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(m => m.PatientId == id && m.AppointEndDate == null);
                if (appointment != null)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == appointment.UserId);
                    if (user != null)
                    {

                        Age age = new(patient.DateofBirth);
                        var currentDate = DateTime.Now;
                        slipViewModel.Name = patient.Name;
                        slipViewModel.FatherName = patient.FatherName ?? "";
                        slipViewModel.Address = patient.Address ?? "";
                        slipViewModel.City = patient.City ?? "";
                        slipViewModel.BloodGroup = patient.BloodGroup;
                        slipViewModel.Doctor = user.Name;
                        slipViewModel.Date = currentDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        slipViewModel.Age = age.AgeString;
                        slipViewModel.PhoneNo = patient.PhoneNo ?? "";
                        slipViewModel.PatientNumber = patient.PatientNumber;
                        slipViewModel.Number = patient.MrNo.ToString("d8",System.Globalization.CultureInfo.InvariantCulture);
                        return slipViewModel;
                    }
                    else
                    {
                        throw new FmdcException("Doctor could not be found");
                    }
                }
                else
                {
                    throw new FmdcException("Appointment could not be found");
                }
            }
            else
            {
                throw new FmdcException("Patient could not be found");
            }

        }
    }
}
