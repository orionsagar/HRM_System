using System.ComponentModel.DataAnnotations;

namespace Application.SupportiveBL.Email
{
    public sealed class SMTPOptions
    {
        public const string SMTPOptionPath = "SMTPOptions";
        [Required(ErrorMessage = "Must provide the host URL")]
        public string Host { get; set; }
        [Required(ErrorMessage = "Must provide the user email")]
        [EmailAddress(ErrorMessage = "Should be a valid email address")]
        public string Email { get; set; }
        public int Port { get; set; } = 587;
        [Required(ErrorMessage = "Must provide the password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Must provide a name")]
        public string Name { get; set; }
    }
}
