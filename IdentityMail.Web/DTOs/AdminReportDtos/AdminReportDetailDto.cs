namespace IdentityMail.Web.DTOs.AdminReportDtos
{
    public class AdminReportDetailDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ReportDate { get; set; }
        public bool IsResolved { get; set; }
        public string ReportedByFullName { get; set; } = string.Empty;
        public string ReportedByEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageBody { get; set; } = string.Empty;
        public DateTime MessageSendTime { get; set; }
        public int SenderId { get; set; }
        public string SenderFullName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverFullName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
    }
}
