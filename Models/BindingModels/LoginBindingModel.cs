using System.ComponentModel.DataAnnotations;

namespace SecureAPI.Models.BindingModels
{
    public class LoginBindingModel
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}