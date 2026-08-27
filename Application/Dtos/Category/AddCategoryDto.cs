using Application.Dtos.Common;

namespace Application.Dtos.Category
{
    public class AddCategoryDto : CreateBaseDto
    {
        public string CategoryName { get; set; }
    }
}