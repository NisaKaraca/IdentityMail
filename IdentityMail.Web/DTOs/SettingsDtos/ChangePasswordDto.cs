using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.SettingsDtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "Yeni şifreler birbiriyle uyuşmuyor."
        )]
        public string ConfirmPassword { get; set; }
    }
}
