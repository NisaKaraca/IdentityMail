using Microsoft.AspNetCore.Identity;

namespace IdentityMail.Web.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public List<UserMessage> SentMessages { get; set; }
            = new List<UserMessage>();
        public List<UserMessage> ReceivedMessages { get; set; }
            = new List<UserMessage>();
        public List<UserMessageCategory> UserMessageCategories { get; set; }
            = new();
        public List<MessageReport> MessageReports { get; set; }
            = new();

    }
}
