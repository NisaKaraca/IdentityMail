namespace IdentityMail.Web.DTOs.AdminUserDtos
{
    public class AdminUserListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SentMessageCount { get; set; }
        public int ReceivedMessageCount { get; set; }
    }
}
