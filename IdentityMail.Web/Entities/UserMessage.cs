namespace IdentityMail.Web.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }
        public AppUser Sender { get; set; }
        public int SenderId { get; set; }
        public AppUser Receiver { get; set; }
        public int ReceiverId { get; set; }
        public bool IsTrashedBySender { get; set; }
        public bool IsTrashedByReceiver { get; set; }
        public bool IsPermanentlyDeletedBySender { get; set; }
        public bool IsPermanentlyDeletedByReceiver { get; set; }
        public List<UserMessageCategory> UserMessageCategories { get; set; }
            = new();
        public List<MessageReport> MessageReports { get; set; }
            = new();
    }
}
