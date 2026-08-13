using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.MessageReportDtos
{
    public class CreateMessageReportDto
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Şikâyet nedeni seçiniz.")]
        public string Reason { get; set; } = string.Empty;

        [StringLength(500,ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }
    }
}
