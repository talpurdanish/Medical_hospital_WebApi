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
    public class TestParametersController : ControllerBase
    {

        private readonly ITestParameterManager _manager;

        public TestParametersController(ITestParameterManager Manager)
        {
            _manager = Manager;

        }
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        // GET: api/<TestParametersController>
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {
            try
            {
                var testParameters = await _manager.GetTestParameters(filter);
                return FmdcResult.Success(testParameters, 200);
            }
            catch (Exception)
            {

                return FmdcResult.Error("TestParameters could not be fetched", 500);
            }
        }

        // GET api/<TestParametersController>/5
        [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id, [FromQuery] string type = "tp")
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("TestParameterId is not valid", 500);
                if (type == "tp")
                {
                    var testParameters = await _manager.GetTestParameter(id);
                    return FmdcResult.Success(testParameters, 200);
                }
                else
                {
                    var testParams = await _manager.GetTestParameters(id);
                    return FmdcResult.Success(testParams, 200);
                }

            }
            catch (Exception)
            {

                return FmdcResult.Error("TestParameter does not exists", 500);
            }
        }

        // POST api/<TestParametersController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] TestParameterViewModel viewModel)
        {
            try
            {
                var result = await _manager.Create(viewModel);
                return FmdcResult.Success("TestParameter has been created", null, 200);
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

        // PUT api/<TestParametersController>
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] TestParameterViewModel viewModel)
        {
            try
            {
                var result = await _manager.Update(viewModel);
                return FmdcResult.Success("TestParameter has been updated", null, 200);
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

        

        // DELETE api/<TestParametersController>/5
        [Authorize(Roles.Administrator, Roles.Staff)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("TestParameterId is not valid", 500);
                var result = await _manager.Delete(id);
                if (result)
                    return FmdcResult.Success("TestParameter has been deleted", null, 200);
                else
                    return FmdcResult.Error("TestParameter has not been deleted", 500);
            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }

    }
}
