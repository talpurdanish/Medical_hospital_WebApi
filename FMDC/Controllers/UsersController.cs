using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Managers.Interfaces;
using FMDC.Security.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = FMDC.Security.Filters.AuthorizeAttribute;

namespace FMDC.Controllers
{
    [Route("api/fmdc/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;

        }
        // GET: api/<UsersController>
        [Authorize(Roles.Administrator, Roles.Doctor, Roles.Staff)]
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] DataFilter filter)
        {

            try
            {
                var users = await _userManager.GetUsers(filter);

                return FmdcResult.Success(users, 200);
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Users could not be fetched", 500);
            }
        }
        [Authorize(Roles.Administrator, Roles.Doctor, Roles.Staff)]
        [HttpGet("[action]")]
        public async Task<JsonResult> GetDoctors()
        {

            try
            {

                var users = await _userManager.GetDoctors();
                var doctors = users.Select(u => new UserValue() { id = u.id, value = u.Name });
                return FmdcResult.Success(doctors, 200);
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Users could not be fetched", 500);
            }
        }



        // GET api/<UsersController>/5
        [Authorize(Roles.Administrator, Roles.Doctor, Roles.Staff)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("UserId is not valid", 500);
                var user = await _userManager.GetUser(id);
                if (user!.Name == Roles.Administrator)
                {
                    return FmdcResult.Error("User does not exists", 500);
                }
                return FmdcResult.Success(user, 200);
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("User does not exists", 500);
            }
        }

        // POST api/<UsersController>
        [Authorize(Roles.Administrator)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] UserViewModel viewModel)
        {
            try
            {
                var signUpResult = await _userManager.SignUp(viewModel);

                return FmdcResult.Success("User has been created", null, 200);
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

        // PUT api/<UsersController>
        [Authorize(Roles.Administrator)]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] UserViewModel viewModel)
        {
            try
            {
                var user = await _userManager.Update(viewModel);
                return FmdcResult.Success(user, 200);
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

        // DELETE api/<UsersController>/5
        [Authorize(Roles.Administrator)]
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return FmdcResult.Error("UserId is not valid", 500);
                var result = await _userManager.Delete(id);
                if (result)
                    return FmdcResult.Success("User has been deleted", null, 200);
                else
                    return FmdcResult.Error("User has not been deleted", 500);
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
        [Authorize(Roles.Administrator)]
        [HttpGet("[action]")]
        public async Task<bool> CheckUsername([FromQuery] string value = "", int id = -1)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return false;
                var signUpResult = await _userManager.CheckDuplicate(DuplicateType.Username, value, id);

                return signUpResult;
            }
            catch (FmdcException)
            {

                return true;
            }
            catch (Exception)
            {

                return true;
            }
        }

        [Authorize(Roles.Administrator)]
        [HttpGet("[action]")]
        public async Task<bool> CheckCNIC([FromQuery] string value, int id = -1)
        {
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(value))
                    return false;
                var signUpResult = await _userManager.CheckDuplicate(DuplicateType.Cnic, value, id);

                return signUpResult;
            }
            catch (FmdcException)
            {

                return true;
            }
            catch (Exception)
            {

                return true;
            }
        }

        [Authorize(Roles.Administrator)]
        [HttpGet("[action]")]
        public async Task<bool> CheckPMDCNo([FromQuery] string value, int id = -1)
        {
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(value))
                    return false;
                var signUpResult = await _userManager.CheckDuplicate(DuplicateType.Pmdcno, value, id);

                return signUpResult;
            }
            catch (FmdcException)
            {

                return true;
            }
            catch (Exception)
            {

                return true;
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<JsonResult> Login(LoginViewModel login)
        {
            try
            {
                var response = await _userManager.Validate(login.Username, login.Password);
                if (response != null && response.Success)
                {
                    Thread.Sleep(500);
                    return
                        FmdcResult.Success("", response, 200);
                }
                else
                {

                    return FmdcResult.Error(response != null && response.Message != null ? response.Message : "Invalid Username/Password", 500);
                }

            }
            catch (Exception e)
            {

                return FmdcResult.Error(e.Message, 500);
            }
        }
        [Authorize(Roles.Administrator)]
        [HttpPost("[action]/{id}")]
        public async Task<JsonResult> ChangeRole(int id, [FromQuery] string value)
        {

            try
            {
                if (id <= 0)
                    return FmdcResult.Error("UserId is not valid", 500);

                if (string.IsNullOrEmpty(value))
                    return FmdcResult.Error("Role cannot be empty", 500);

                if (Roles.CheckValidity(value))
                {
                    return FmdcResult.Error("Role is not valid", 500);
                }

                var result = await _userManager.AddToRole(id, value);


                return FmdcResult.Success("Role has been changed", null, 200);

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
        [Authorize(Roles.Administrator)]
        [HttpPost("[action]/{id}")]
        public async Task<JsonResult> ChangeStatus(int id)
        {

            try
            {
                var result = await _userManager.ChangeUserStatus(id);

                return FmdcResult.Success("User Status has been changed", 200);

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
        [Authorize(Roles.Administrator)]
        [HttpPost("[action]/{id}")]
        public async Task<JsonResult> ResetPassword(int id)
        {

            try
            {
                if (id <= 0)
                    return FmdcResult.Error("UserId is not valid", 500);


                var result = await _userManager.ResetSecret(id);


                return FmdcResult.Success("Password has been reset", null, 200);

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

    }
}
