using Application.Dtos.Common;

namespace Application.Dtos.Category
{
    public class UpdateCategoryDto : UpdateBaseDto
    {
        public string CategoryName { get; set; }
    }
}