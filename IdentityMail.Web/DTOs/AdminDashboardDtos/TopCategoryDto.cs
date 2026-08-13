namespace IdentityMail.Web.DTOs.AdminDashboardDtos
{
    public class TopCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public double Percentage { get; set; }
    }
}
