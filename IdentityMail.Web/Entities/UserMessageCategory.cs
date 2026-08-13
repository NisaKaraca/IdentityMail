namespace IdentityMail.Web.Entities
{
    public class UserMessageCategory
    {
        public int Id { get; set; }
        public int UserMessageId { get; set; }
        public UserMessage UserMessage { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
