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
    [Authorize(Roles.Administrator, Roles.Staff, Roles.Doctor)]
    public class TodosController : ControllerBase
    {
        private readonly ITodoManager _manager;

        public TodosController(ITodoManager Manager)
        {
            _manager = Manager;

        }
        // GET: api/<TodosController>
        
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            try
            {
                var user = GetCurrentUser();
                if (user is not null)
                {
                    var todos = await _manager.GetTodos(user.id);
                    return FmdcResult.Success("", todos, 200);
                }
                else
                {
                    return FmdcResult.Error("User cannot be found", 500);
                }
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Todos could not be fetched", 500);
            }
        }
        
        // GET api/<TodosController>/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {

            try
            {
                var user = GetCurrentUser();
                if (user is not null)
                {
                    var todo = await _manager.GetTodo(user.id);
                    return FmdcResult.Success("", todo, 200);
                }
                else
                {
                    return FmdcResult.Error("User cannot be found", 500);
                }

            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Todo could not be fetched", 500);
            }
        }
        
        // POST api/<TodosController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] string title)
        {
            try
            {
                var user = GetCurrentUser();
                if (user is not null)
                {
                    var message = await _manager.Create(user.id, title);
                    return FmdcResult.Success(message, 200);
                }
                else
                {
                    return FmdcResult.Error("User cannot be found", 500);
                }

            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Todo could not be fetched", 500);
            }
        }
        
        // PUT api/<TodosController>/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody] string title)
        {
            try
            {
                var message = await _manager.Update(id, title);
                return FmdcResult.Success(message, 200);
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Todo could not be fetched", 500);
            }
        }
        
        // DELETE api/<TodosController>/5
        [HttpPost("[action]")]
        public async Task<JsonResult> Manage(ManageTodoViewModel viewModel)
        {
            try
            {
                var message = "";
                var ids = StringToArray(viewModel.ids);
                switch ((TaskType)viewModel.type)
                {
                    case TaskType.MARK:
                        message = await _manager.Mark(ids);

                        break;
                    case TaskType.DELETE:
                        message = await _manager.Delete(ids);
                        break;

                }
                return FmdcResult.Success(message, 200);
            }
            catch (FmdcException ae)
            {
                return FmdcResult.Error(ae.Message, 500);
            }
            catch (Exception)
            {

                return FmdcResult.Error("Todo could not be fetched", 500);
            }


        }

        private static int[] StringToArray(string arrayString)
        {
            int[] rInt;
            var onlyInt = 0;

            arrayString = arrayString.Replace('[', ' ').Replace(']', ' ');
            if (int.TryParse(arrayString, out onlyInt))
            {
                rInt = new int[] { onlyInt };
            }
            else
            {

                string[] aStr = arrayString.Split(',');
                rInt = new int[aStr.Length];
                int i = 0;
                foreach (var str in aStr)
                {
                    rInt[i] = int.Parse(str,System.Globalization.CultureInfo.InvariantCulture);
                    i++;
                }
            }
            return rInt;
        }
        private UserViewModel? GetCurrentUser()
        {
            return HttpContext.Items["User"] as UserViewModel;

        }
    }
    enum TaskType
    {
        MARK = 1, DELETE = 2
    }
}
