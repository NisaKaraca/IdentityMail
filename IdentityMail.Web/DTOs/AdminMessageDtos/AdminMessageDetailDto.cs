namespace IdentityMail.Web.DTOs.AdminMessageDtos
{
    public class AdminMessageDetailDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }
        public string SenderFullName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string? SenderImageUrl { get; set; }
        public string ReceiverFullName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string? ReceiverImageUrl { get; set; }
        public bool IsTrashedBySender { get; set; }
        public bool IsTrashedByReceiver { get; set; }
        public int ReportCount { get; set; }
    }
}
