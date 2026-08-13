namespace IdentityMail.Web.DTOs.AdminMessageDtos
{
    public class AdminMessageListDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string SenderFullName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverFullName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }
        public bool IsInTrash { get; set; }
        public int ReportCount { get; set; }
    }
}
