namespace IdentityMail.Web.DTOs.AdminDashboardDtos
{
    public class TopSenderDto
    {
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public int MessageCount { get; set; }
    }
}
