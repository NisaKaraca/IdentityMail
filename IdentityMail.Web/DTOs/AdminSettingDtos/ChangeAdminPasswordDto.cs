using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.AdminSettingDtos
{
    public class ChangeAdminPasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "Yeni şifreler birbiriyle uyuşmuyor.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
