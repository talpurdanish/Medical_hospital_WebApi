using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Reports;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Helpers;

namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {

        private readonly IPatientManager _manager;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public PatientsController(IPatientManager manager, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            _manager = manager;
            _environment = environment;
        }
        // GET: api/<PatientsController>
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var patients = await _manager.GetPatients(filter);

                return FmdcResult.Success("", patients, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Patients could not be fetched", 500);
            }
        }

        // GET api/<PatientsController>/5
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("PatientId is not valid", 500);
                var patient = await _manager.GetPatient(id);
                return FmdcResult.Success("", patient, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Patient does not exists", 500);
            }
        }

        // POST api/<PatientsController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] PatientViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("Patient has been created", null, 200);
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

        // PUT api/<PatientsController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] PatientViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("Patient has been updated", null, 200);
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

        // DELETE api/<PatientsController>/5
        [Authorize(Roles.Administrator)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("PatientId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("Patient has been deleted", null, 200);
                else
                    return FmdcResult.Error("Patient has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpGet("[action]")]
        public async Task<bool> CheckCNIC([FromQuery] string value, int id = -1)
        {
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(value))
                    return false;
                var signUpResult = await _manager.CheckDuplicate(DuplicateType.Cnic, value, id);

                return signUpResult;
            }
            catch (Exception)
            {

                return true;
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]")]
        public async Task<JsonResult> GetStats()
        {
            try
            {
                var stats = await _manager.GetStat();
                return FmdcResult.Success("", stats, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("No Stats Available", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetButtons(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("PatientId is not valid", 500);
                var stats = await _manager.GetButtons(id);
                return FmdcResult.Success("", stats, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("No Buttons Available", 500);
            }
        }

        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GenerateSlip(int id)
        {
            var slip = await _manager.GenerateSlip(id);
            var path = _environment.ContentRootPath + "/Reports";
            var reportGenerator = new PdfReportGenerator();
            var slipType = new ReportType("slip.pdf",297f,210f, slip );

            if (reportGenerator.Generate(path, slipType))
            {
                var stream = new FileStream(path + "/slip.pdf", FileMode.Open);
                //var file = ConvertToBase64(stream);
                //string slipTitle = string.Format("{0} (Mr No: {1})", slip.Name, slip.PatientNumber);
                //stream.Close();
                //return FmdcResult.Success(slipTitle, file, 200);
                //byte[] bytes = System.IO.File.ReadAllBytes(path + "/slip.pdf");
                //return File(bytes, "application/pdf");
                return new FileStreamResult(stream, "application/pdf");
            }
            else
            {
                return FmdcResult.Error("Pdf could not been generated", 500);
            }

        }
        private static string ConvertToBase64(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return Convert.ToBase64String(memoryStream.ToArray());
            }

            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            stream.Close();
            return Convert.ToBase64String(bytes);
        }
    }
}
