using Domain.Helpers;
using Domain.Models;
using FMDC.Managers.Interfaces;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    [Authorize(Roles.Administrator, Roles.Staff,Roles.Doctor)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityManager _manager;

        public CitiesController(ICityManager manager)
        {
            _manager = manager;
        }

        // GET: api/<CitiesController>
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            try
            {
                var Cities =await _manager.GetCities();
                return FmdcResult.Success("", Cities, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("Cities could not be fetched", 500);
            }
        }

         [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                var Cities =await _manager.GetCities(id);
                return FmdcResult.Success("", Cities, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("Cities could not be fetched", 500);
            }
        }


    }
}
