namespace IdentityMail.Web.DTOs.SettingsDtos
{
    public class SettingsPageDto
    {
        public UpdateProfileDto Profile { get; set; }
           = new UpdateProfileDto();
        public ChangePasswordDto Password { get; set; }
            = new ChangePasswordDto();
    }
}
