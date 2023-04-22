using Microsoft.Extensions.Configuration.UserSecrets;

namespace Domain.Viewmodels
{
    public class ChangePasswordViewModel
    {

        public int UserId{get; set;}
        public string Oldpassword  {get;set;} = "";
        public string Newpassword  {get;set;} = "";
    }
}
