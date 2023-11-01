#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;

namespace EntertainmentGuild.DTO
{
    public class UserDTO
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAdress { get; set; }
        [Required(ErrorMessage = "Postal Code will be 1232 like")]
        [Range(1, 9999, ErrorMessage = "Postal Code will be 1232 like")]
        public string PostalCode { get; set; }
        [Required]
        public string Suburb { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string? CardNumber { get; set; }
        [Required]
        public string? CardOwner { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public string? ExpiryDate { get; set; }
        [Required(ErrorMessage = "CVV is required")]
        public int? CVV { get;set; }

    }
}
