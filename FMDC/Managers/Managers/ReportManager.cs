using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace FMDC.Managers.Managers
{
    public class ReportManager : IReportManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public ReportManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<LabReportViewModel?> GetReport(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from lr in _context.LabReports
                        join t in _context.Tests on lr.TestId equals t.Id
                        join p in _context.Patients on lr.PatientId equals p.PatientId
                        join u in _context.Users on lr.DoctorId equals u.UserId
                        where lr.Id == id
                        select new LabReportViewModel
                        {
                            PatientId = p.PatientId,
                            ReportDate = lr.ReportDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            ReportTime = $@"{lr.ReportTime:hh\:mm}",
                            ReportDeliveryDate = lr.ReportDeliveryDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            ReportDeliveryTime = $@"{lr.ReportDeliveryTime:hh\:mm}",
                            TestId = lr.TestId,
                            DoctorId = lr.DoctorId,
                            Id = lr.Id,
                            TestName = t.Name,
                            PatientName = p.Name,
                            PatientNumber = p.PatientNumber,
                            Doctor = u.Name,
                            Status = GetReportStatus(_context, lr.Id, lr.TestId),
                            PatientAge = new Age(p.DateofBirth).AgeString,
                            PatientGender = p.Gender ?? "0",
                            DoctorPMDCNo = u.PMDCNo,
                            ReportNoString = lr.ReportNumber.ToString("D8", System.Globalization.CultureInfo.InvariantCulture),
                            Note = lr.Note,
                        };
            var labReport = await query.FirstOrDefaultAsync();
            if (labReport != null)
            {
                var parameters = _context.TestParameters.Where(p => p.TestId == labReport.TestId);
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        StatusValuePair statusValuePair = await GetStatusValuePair(param.Id, labReport.Id);
                        labReport.TestParameters.Add(TestParameterViewModel.GenerateViewModel(param, statusValuePair.Status, statusValuePair.Value));
                    }
                }
            }

            return labReport;

        }

        private async Task<IEnumerable<LabReportViewModel>> FetchReports()
        {
            var query = from lr in _context.LabReports
                        join t in _context.Tests on lr.TestId equals t.Id
                        join p in _context.Patients on lr.PatientId equals p.PatientId
                        join u in _context.Users on lr.DoctorId equals u.UserId
                        select new LabReportViewModel
                        {
                            PatientId = p.PatientId,
                            ReportDate = lr.ReportDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            ReportTime = $@"{lr.ReportTime:hh\:mm}",
                            ReportDeliveryDate = lr.ReportDeliveryDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            ReportDeliveryTime = $@"{lr.ReportDeliveryTime:hh\:mm}",
                            TestId = lr.TestId,
                            DoctorId = lr.DoctorId,
                            Id = lr.Id,
                            TestName = t.Name,
                            PatientName = p.Name,
                            PatientNumber = p.PatientNumber,
                            Doctor = u.Name,
                            Status = GetReportStatus(_context, lr.Id, lr.TestId),
                            PatientAge = new Age(p.DateofBirth).AgeString,
                            PatientGender = p.Gender ?? "0",
                            DoctorPMDCNo = u.PMDCNo,
                            ReportNoString = lr.ReportNumber.ToString("D8", System.Globalization.CultureInfo.InvariantCulture),
                            Note = lr.Note,

                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<LabReportViewModel>> GetReports(DataFilter filter)
        {

            var Reports = await FetchReports();
            return Sort(filter.SortField, filter.Order, Search(filter.Term ?? "", Reports));


        }
        private static IEnumerable<LabReportViewModel> Search(string term, IEnumerable<LabReportViewModel> Report)
        {
            IEnumerable<LabReportViewModel> Reports = new List<LabReportViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return Report;
            }
            else
            {
                var query = from u in Report
                            where
                           u.TestName!.Contains(term) || u.PatientName!.Contains(term) || u.Doctor!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<LabReportViewModel> Sort(int field, int order, IEnumerable<LabReportViewModel> list)
        {
            IEnumerable<LabReportViewModel> listO = new List<LabReportViewModel>();


            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.ReportDate) : list.OrderByDescending(p => p.ReportDate),
                2 => order == 1 ? list.OrderBy(p => p.ReportTime) : list.OrderByDescending(p => p.ReportTime),
                3 => order == 1 ? list.OrderBy(p => p.ReportDeliveryDate) : list.OrderByDescending(p => p.ReportDeliveryDate),
                4 => order == 1 ? list.OrderBy(p => p.ReportDeliveryTime) : list.OrderByDescending(p => p.ReportDeliveryTime),
                5 => order == 1 ? list.OrderBy(p => p.TestName) : list.OrderByDescending(p => p.TestName),
                6 => order == 1 ? list.OrderBy(p => p.PatientName) : list.OrderByDescending(p => p.PatientName),
                7 => order == 1 ? list.OrderBy(p => p.Doctor) : list.OrderByDescending(p => p.Doctor),
                8 => order == 1 ? list.OrderBy(p => p.ReportNumber) : list.OrderByDescending(p => p.ReportNumber),
                9 => order == 1 ? list.OrderBy(p => p.Status) : list.OrderByDescending(p => p.Status),
                _ => list,
            };
            return listO;
        }
        public async Task<IEnumerable<LabReportViewModel>> GetPatientReports(int id)
        {

            var reports = await FetchReports();
            reports = reports.Where(p => p.PatientId == id);
            return reports;


        }




        public async Task<string> Update(LabReportViewModel viewmodel)
        {
            var report = await _context.LabReports.FindAsync(viewmodel.Id);
            if (report is null)
                throw new FmdcException("Report could not be found");

            var now = DateTime.Now;
            var nowDate = now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            var nowTime = now.ToString("HH:mm", CultureInfo.InvariantCulture);
            TimeSpan reportTime = TimeSpan.ParseExact(viewmodel.Id > 0 ? viewmodel.ReportTime : nowTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);
            TimeSpan deliveryTime = TimeSpan.ParseExact(viewmodel.ReportDeliveryTime, new string[] { "hhmm", @"hh\:mm" }, CultureInfo.InvariantCulture);

            report.PatientId = viewmodel.PatientId;
            report.ReportDate = DateOnly.ParseExact(nowDate, "dd/MM/yyyy", null, DateTimeStyles.None);
            report.ReportTime = reportTime;
            report.ReportDeliveryDate = DateOnly.ParseExact(viewmodel.ReportDeliveryDate, "dd/MM/yyyy", null, DateTimeStyles.None);
            report.ReportDeliveryTime = deliveryTime;
            report.TestId = viewmodel.TestId;
            report.DoctorId = viewmodel.DoctorId;

            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "Report has been updated";
        }

        public async Task<string> UpdateValues(AddReportValues reportValues)
        {

            var rvs = GenerateReportValuesList(reportValues.Paramvalues, reportValues.LabReportId);
            if (rvs is null || rvs.Count <= 0)
                throw new FmdcException("Invalid Report Values");
            await _context.ReportValues.AddRangeAsync(rvs);
            var valuesupdated = await _context.SaveChangesAsync();
            return valuesupdated > 0 ? "Report has been updated" : "report has not been updated";


        }

        public async Task<IEnumerable<TestParameter>> GetPendingParameters(int id)
        {
            var report = await _context.LabReports.FindAsync(id);
            if (report is null)
                throw new FmdcException("Report could not be found");
            var reportQuery = from rv in _context.ReportValues
                              where rv.LabReportId == id
                              select rv.TestParameterId;
            var query = from tp in _context.TestParameters
                        where tp.TestId == report.TestId && !reportQuery.Contains(tp.Id)
                        select tp;
            var list = await query.ToListAsync();
            return list;

        }

        private static IList<ReportValue> GenerateReportValuesList(string arrayString, int lId)
        {

            //arrayString = arrayString.Replace('[', ' ').Replace(']', ' ').Replace("\"", "");

            IList<ReportValue> list = new List<ReportValue>();
            if (!string.IsNullOrEmpty(arrayString))
            {

                JArray array = JArray.Parse(arrayString);
                foreach (JObject obj in array.Children<JObject>())
                {
                    var id = 0;
                    foreach (JProperty singleProp in obj.Properties())
                    {

                        var name = singleProp.Name;
                        var value = singleProp.Value;
                        if (name == "id")
                        {
                            id = (int)value;
                            continue;
                        }
                        //Do something with name and value
                        //System.Windows.MessageBox.Show("name is "+name+" and value is "+value);
                        list.Add(new ReportValue()
                        {
                            Value = double.Parse(value.ToString(), System.Globalization.CultureInfo.InvariantCulture),
                            TestParameterId = id,
                            LabReportId = lId
                        });
                    }
                }
                //string[] aStr = arrayString.Split(',');
                //foreach (var str in aStr)
                //{

                //    var strArray = str.Split(':');
                //    if (strArray.Length == 2)
                //    {

                //    }
                //}
            }
            return list;
        }

        public async Task<string> Create(LabReportViewModel viewmodel)
        {
            var previousId = _context.LabReports.Max(lr => lr.ReportNumber);
            var model = LabReportViewModel.GenerateModel(viewmodel, previousId);

            _context.LabReports.Add(model);
            await _context.SaveChangesAsync();
            return "Report has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var report = await _context.LabReports.FindAsync(id);
                if (report == null)
                {
                    return false;
                }


                var reportValues = await _context.ReportValues.Where(rv => rv.LabReportId == id).ToListAsync();
                if (reportValues != null)
                {
                    _context.ReportValues.RemoveRange(reportValues);
                }
                _context.Entry(report).State = EntityState.Deleted;
                _context.LabReports.Remove(report);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static string GetReportStatus(MedicalContext context, int id, int testId)
        {


            var reportValues = context.ReportValues.Where(p => p.LabReportId == id).ToList();
            if (reportValues != null)
            {
                var parametercount = context.TestParameters.Where(p => p.TestId == testId).Count();
                if (reportValues.Count == parametercount)
                {
                    return "Completed";
                }

            }
            return "Pending";
        }

        private async Task<StatusValuePair> GetStatusValuePair(int id, int reportid)
        {
            if (id > 0 && reportid > 0)
            {
                var value = await _context.ReportValues.FirstOrDefaultAsync(rv => rv.TestParameterId == id && rv.LabReportId == reportid);

                return new StatusValuePair()
                {
                    Status = value == null,
                    Value = value != null ? value.Value : 0.0
                };

            }
            return new StatusValuePair()
            {
                Status = true,
                Value = 0.0
            };
        }

        public async Task<IEnumerable<LabReportViewModel>> GetPendingReports()
        {
            var reports = await FetchReports();
            reports = reports.Where(p => p.Status == "Pending");
            return reports;
        }
    }
    internal sealed class StatusValuePair
    {
        public bool Status { get; set; }
        public double Value { get; set; }
    }
}
