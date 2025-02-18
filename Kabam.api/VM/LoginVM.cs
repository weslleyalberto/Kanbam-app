using System.ComponentModel.DataAnnotations;

namespace Kabam.api.VM
{
    public struct LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
