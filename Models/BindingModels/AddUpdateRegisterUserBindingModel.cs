namespace SecureAPI_Identity_EF.Models.BindingModels
{
    public class AddUpdateRegisterUserBindingModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}