using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityMail.Web.DTOs.MessageCategoryDtos
{
    public class AssignMessageCategoryDto
    {
        public int MessageId { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
    }
}
