using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Mvc;



namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {

        private readonly IMedicationManager _manager;
        
        public MedicationsController(IMedicationManager Manager)
        {
            _manager = Manager;
            
        }
        // GET: api/<MedicationsController>
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var Medications = await _manager.GetMedications(filter);

                return FmdcResult.Success(Medications, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Medications could not be fetched", 500);
            }
        }

        // GET api/<MedicationsController>/5
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("MedicationId is not valid", 500);
                var Medication = await _manager.GetMedication(id);
                return FmdcResult.Success(Medication, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Medication does not exists", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Staff)]
        // POST api/<MedicationsController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] MedicationViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("Medication has been created", null, 200);
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
        // PUT api/<MedicationsController>
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] MedicationViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("Medication has been updated", null, 200);
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
        // DELETE api/<MedicationsController>/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("MedicationId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("Medication has been deleted", null, 200);
                else
                    return FmdcResult.Error("Medication has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

    }
}
