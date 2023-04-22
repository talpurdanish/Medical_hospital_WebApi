namespace Domain.Models
{
    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Doctor = "Doctor";
        public const string Staff = "Staff";


        public static bool CheckValidity(string value)
        {
            return value != Administrator && value != Doctor && value != Staff;
        }
    }
}
