using System.ComponentModel.DataAnnotations;

namespace Lumia.ViewModels.AccountVM
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Adivi Yaaaaaz"),MaxLength(20)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Adivi Yazdin,Soyadin Qaldi"), MaxLength(24)]
        public string Surname { get; set; } = null!;
        public string UserName { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
