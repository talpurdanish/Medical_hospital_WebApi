using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(250)]
        [NotNull]
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string PMDCNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public byte[] Picture { get; set; } = new byte[1];
        public string Gender { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public int PhoneType { get; set; }
        public DateTime DateofBirth { get; set; }
        [Required]
        public string CNIC { get; set; } = string.Empty;

        public int CityId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PasswordSalt { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;


        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }

    
}
