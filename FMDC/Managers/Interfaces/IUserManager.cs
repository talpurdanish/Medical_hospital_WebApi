using Domain.Helpers;
using Domain.Viewmodels;


namespace FMDC.Managers.Interfaces
{

    public enum ValidateErrors
    {
        UserIsNotActive,
        SecretNotValid,
        NoError
    }


    public enum DuplicateType
    {
        Username,
        Cnic,
        Pmdcno
    }
    public class ValidateResult
    {

        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public string Token { get; set; } = "";
        public string Created { get; set; } = "";
        public bool IsActive { get; set; }
        public string Picture { get; set; } = "";
        public bool Success { get; set; }
        public string Message { get; set; } = "";


        public ValidateResult(UserViewModel? user = null, string jwtToken = "", bool success = false, ValidateErrors error = ValidateErrors.NoError)
        {
            Success = success;
            if (success && user != null)
            {
                Id = user.id;
                FirstName = user.Name is not null ? user.Name : "";
                Username = user.Username is not null ? user.Username : "";
                Token = jwtToken;
                Created = user.Created.ToShortDateString();
                IsActive = user.IsActive == null && (bool)user.IsActive!;
                Role = user.Role;
                Picture = user.Picture == null ? "" : user.Picture;
                Message = "";
            }
            else
            {
                Message = error == ValidateErrors.UserIsNotActive ? "User is not active" : "Invalid Username/Password";
            }
        }
    }

    public interface IUserManager
    {

        Task<UserViewModel?> GetUser(int id, bool deleted = false);
        Task<IEnumerable<UserViewModel>> GetDoctors();
        Task<IEnumerable<UserViewModel>> GetUsers(DataFilter filter);

        Task<string> SignUp(UserViewModel viewmodel);
        Task<string> Update(UserViewModel viewmodel);
        Task<bool> Delete(int id);

        Task<bool> AddToRole(int id, string roleCode);
        Task<bool> AddToRole(int id);
        Task<string> ChangeSecret(string identifier, string secret, string newsecret);
        Task<ValidateResult> Validate(string identifier, string secret);
        bool SignOut();
        Task<bool> ChangeUserStatus(int id);
        Task<string> ResetSecret(int id);

        Task<bool> CheckDuplicate(DuplicateType type, string value, int id = -1);
    }
}
