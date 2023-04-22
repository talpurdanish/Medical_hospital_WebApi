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
    public class ProcedureTypesController : ControllerBase
    {

        private readonly IProcedureTypeManager _manager;
        
        public ProcedureTypesController(IProcedureTypeManager Manager)
        {
            _manager = Manager;
            
        }
        // GET: api/<ProcedureTypesController>
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var ProcedureTypes = await _manager.GetProcedureTypes(filter);

                return FmdcResult.Success(ProcedureTypes, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("ProcedureTypes could not be fetched", 500);
            }
        }

        // GET api/<ProcedureTypesController>/5
        [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ProcedureTypeId is not valid", 500);
                var ProcedureType = await _manager.GetProcedureType(id);
                return FmdcResult.Success(ProcedureType, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("ProcedureType does not exists", 500);
            }
        }

        // POST api/<ProcedureTypesController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] ProcedureTypeViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("ProcedureType has been created", null, 200);
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

        // PUT api/<ProcedureTypesController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] ProcedureTypeViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("ProcedureType has been updated", null, 200);
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
        // DELETE api/<ProcedureTypesController>/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("ProcedureTypeId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("ProcedureType has been deleted", null, 200);
                else
                    return FmdcResult.Error("ProcedureType has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

    }
}
