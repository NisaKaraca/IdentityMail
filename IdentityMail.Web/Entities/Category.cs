namespace IdentityMail.Web.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#2563eb";
        public string Icon { get; set; } = "label";
        public bool IsActive { get; set; } = true;
        public List<UserMessageCategory> UserMessageCategories { get; set; } = new();
    }
}
