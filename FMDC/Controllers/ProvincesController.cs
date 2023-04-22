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
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceManager _manager;

        public ProvincesController(IProvinceManager manager)
        {
            _manager = manager;
        }

        // GET: api/<ProvincesController>
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            try
            {
                var provinces =await _manager.GetProvinces();
                return FmdcResult.Success("", provinces, 200);
            }
            catch (Exception)
            {
                return FmdcResult.Error("Provinces could not be fetched", 500);
            }
        }


    }
}
