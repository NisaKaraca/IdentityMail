namespace IdentityMail.Web.DTOs.AdminReportDtos
{
    public class AdminReportListDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string SenderFullName { get; set; } = string.Empty;
        public string ReportedByFullName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public bool IsResolved { get; set; }
    }
}
