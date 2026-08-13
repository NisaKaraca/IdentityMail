using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class SendMailDto
    {
        public int? DraftId { get; set; }

        [Required(ErrorMessage = "Alıcı e-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string ReceiverMail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu alanı zorunludur.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj içeriği zorunludur.")]
        public string Body { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
    }
}
