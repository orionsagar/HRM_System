using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    public class ExternalClientVM
    {

        [Display(Name = "Organisation Name")]
        [Required(ErrorMessage = "Organisation Name is required.")]
        public string OrgName { get; set; }
        public string SRACode { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Privacy Policy")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "agree to the Privacy Policy.")]
        public bool Privacy_Policy { get; set; }
        [Display(Name = "Understand")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "Give consent by understanding the below term")]
        public bool Understand { get; set; }
        public bool IsOrg { get; set; }
        public bool IsSolicitor { get; set; }
        public string RegType { get; set; }
        public int? UserId { get; set; }
        public int? ClientId { get; set; }
        public int? OrgId { get; set; }
        public string? OrgCode { get; set; }
    }
}
