using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Managers.Managers;
using FMDC.Reports;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Mvc;



namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IReportManager _manager;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        public ReportsController(IReportManager manager, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            _manager = manager;
            _environment = environment;
            
        }
        // GET: api/<ReportsController>
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var reports = await _manager.GetReports(filter);

                return FmdcResult.Success(reports, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Reports could not be fetched", 500);
            }
        }

        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("[action]")]
        public async Task<JsonResult> GetPending()
        {

            try
            {
                var reports = await _manager.GetPendingReports();

                return FmdcResult.Success(reports, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Reports could not be fetched", 500);
            }
        }

        // GET api/<ReportsController>/5
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ReportId is not valid", 500);
                var report = await _manager.GetReport(id);
                return FmdcResult.Success(report, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Report does not exists", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetPatientReports(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ReportId is not valid", 500);
                var reports = await _manager.GetPatientReports(id);
                return FmdcResult.Success(reports, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Report does not exists", 500);
            }
        }

        // POST api/<ReportsController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] LabReportViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("Report has been created", null, 200);
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

        // PUT api/<ReportsController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] LabReportViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("Report has been updated", null, 200);
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
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetPendingParameters(int id){
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ReportId is not valid", 500);
                var reports = await _manager.GetPendingParameters(id);
                return FmdcResult.Success(reports, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Report does not exists", 500);
            }

            }
        [Authorize(Roles.Administrator, Roles.Staff)]
         [HttpPut("[action]")]
        public async Task<JsonResult> UpdateValues([FromBody] AddReportValues reportValues)
        {
            try
            {

                var result = await _manager.UpdateValues(reportValues);
                return FmdcResult.Success(result, 200);
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

        // DELETE api/<ReportsController>/5
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ReportId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("Report has been deleted", null, 200);
                else
                    return FmdcResult.Error("Report has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
         [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GenerateReport(int id)
        {
            var report = await _manager.GetReport(id);
            var path = _environment.ContentRootPath + "/Reports";
            var reportGenerator = new PdfReportGenerator();
            var type = new ReportType("report.pdf", 297f, 210f, report!);

            if (reportGenerator.Generate(path, type))
            {
                var stream = new FileStream(path + "/report.pdf", FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");
            }
            else
            {
                return FmdcResult.Error("Pdf could not been generated", 500);
            }

        }

    }
}
