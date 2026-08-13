namespace IdentityMail.Web.Entities
{
    public class MailDraft
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public AppUser Owner { get; set; }
        public string? ReceiverMail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
