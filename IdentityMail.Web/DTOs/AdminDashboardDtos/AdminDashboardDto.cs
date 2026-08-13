namespace IdentityMail.Web.DTOs.AdminDashboardDtos
{
    public class AdminDashboardDto
    {
        public int TotalUserCount { get; set; }
        public int ActiveUserCount { get; set; }
        public int TotalMessageCount { get; set; }
        public int TodayMessageCount { get; set; }
        public int UnreadMessageCount { get; set; }
        public int TrashMessageCount { get; set; }
        public List<TopSenderDto> TopSenders { get; set; } = new();
        public List<TopCategoryDto> TopCategories { get; set; } = new();
        public int PendingReportCount { get; set; }
    }
}
