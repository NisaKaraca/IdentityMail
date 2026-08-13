namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class SaveDraftDto
    {
        public int? DraftId { get; set; }
        public string? ReceiverMail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
