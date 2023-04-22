using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Reports;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class RecieptsController : ControllerBase
    {

        private readonly IRecieptManager _manager;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public RecieptsController(IRecieptManager Manager, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            _manager = Manager;
            _environment = environment;
        }
        // GET: api/<RecieptsController>
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var Reciepts = await _manager.GetReciepts(filter);

                return FmdcResult.Success("", Reciepts, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Reciepts could not be fetched", 500);
            }
        }

        // GET api/<RecieptsController>/5
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("RecieptId is not valid", 500);
                var reciept = await _manager.GetReciept(id);
                return FmdcResult.Success(reciept, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Reciept does not exists", 500);
            }
        }
        
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("[action]")]
        public async Task<JsonResult> GetUnpaidReciepts()
        {
            try
            {
                var reciepts = await _manager.GetUnpaidReciepts();
                return FmdcResult.Success("", reciepts, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("reciepts could not be fetched", 500);
            }
        }
        
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetPatientReciepts(int id)
        {
            try
            {
                var reciepts = await _manager.GetPatientReciepts(id);
                return FmdcResult.Success("", reciepts, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("reciepts could not be fetched", 500);
            }
        }
        
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        
        [HttpGet("[action]/{id}")]
        public async Task<JsonResult> GetRecieptProcedures(int id)
        {
            try
            {
                var procedures = await _manager.GetRecieptProcedures(id);
                return FmdcResult.Success(procedures, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Procedures could not be fetched", 500);
            }
        }


        // POST api/<RecieptsController>
        
        [Authorize(Roles.Administrator, Roles.Staff)]
        
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] RecieptViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("Reciept has been created", null, 200);
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

        // PUT api/<RecieptsController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id)
        {
            try
            {
                var result = await _manager.UpdatePaidStatus(id);
                return FmdcResult.Success("Reciept has been updated", 200);
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

        // DELETE api/<RecieptsController>/5
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("RecieptId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("Reciept has been deleted", null, 200);
                else
                    return FmdcResult.Error("Reciept has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
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

        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GenerateReciept(int id)
        {
            var reciept = await _manager.GenerateReciept(id);
            if (reciept is not null)
            {
                var type = new ReportType("reciept.pdf",170f,210f, reciept );

                var path = _environment.ContentRootPath + "/Reports";
                var reportGenerator = new PdfReportGenerator();

                if (reportGenerator.Generate(path, type))
                {
                    var stream = new FileStream(path + "/reciept.pdf", FileMode.Open);
                    return new FileStreamResult(stream, "application/pdf");
                }
                else
                {
                    return FmdcResult.Error("Pdf could not been generated", 500);
                }
            }
            else
            {
                return FmdcResult.Error("Reciept could not be found", 500);
            }

        }



        private UserViewModel? GetCurrentUser()
        {

            var user = (UserViewModel?)HttpContext.Items["User"];
            return user;

        }
    }
}
