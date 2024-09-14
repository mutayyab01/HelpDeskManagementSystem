using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} Characters Long", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} Characters Long", MinimumLength = 6)]
        [Compare("Password",ErrorMessage ="The Passsword and the Confirmation password do not Match")]
        public string ConfirmPassword { get; set; }
        [DisplayName("Email Address")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("User Full Name")]

        public string FullName { get; set; }

        public string Id { get; set; }
        [DisplayName("User Role")]
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Gender")]
        public int GenderId { get; set; }
        public SystemCodeDetail Gender { get; set; }

    }
}
