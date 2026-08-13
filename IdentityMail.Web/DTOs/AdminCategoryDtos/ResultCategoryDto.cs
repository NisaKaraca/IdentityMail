namespace IdentityMail.Web.DTOs.AdminCategoryDtos
{
    public class ResultCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int UsageCount { get; set; }
    }
}
