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
    public class MedicationTypesController : ControllerBase
    {

        private readonly IMedicationTypeManager _manager;
        
        public MedicationTypesController(IMedicationTypeManager Manager)
        {
            _manager = Manager;
            
        }
        // GET: api/<MedicationTypesController>
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var MedicationTypes = await _manager.GetMedicationTypes(filter);

                return FmdcResult.Success(MedicationTypes, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("MedicationTypes could not be fetched", 500);
            }
        }

        // GET api/<MedicationTypesController>/5
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("MedicationTypeId is not valid", 500);
                var MedicationType = await _manager.GetMedicationType(id);
                return FmdcResult.Success(MedicationType, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("MedicationType does not exists", 500);
            }
        }

        // POST api/<MedicationTypesController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] MedicationTypeViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("MedicationType has been created", null, 200);
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

        // PUT api/<MedicationTypesController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] MedicationTypeViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("MedicationType has been updated", null, 200);
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

        // DELETE api/<MedicationTypesController>/5
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("MedicationTypeId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("MedicationType has been deleted", null, 200);
                else
                    return FmdcResult.Error("MedicationType has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

    }
}
