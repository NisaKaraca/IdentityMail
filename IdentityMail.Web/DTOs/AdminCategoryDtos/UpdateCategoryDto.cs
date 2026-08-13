using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.AdminCategoryDtos
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50,ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kategori rengi zorunludur.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori ikonu zorunludur.")]
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
