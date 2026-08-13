namespace IdentityMail.Web.DTOs.AdminSettingDtos
{
    public class AdminSettingsDto
    {
        public UpdateAdminProfileDto Profile { get; set; } = new();
        public ChangeAdminPasswordDto Password { get; set; } = new();
    }
}
