using Application.Dtos.Category;
using MediatR;

namespace Application.Features.Category.Requests.Commands
{
    public class AddCategoryCommand : IRequest
    {
        public AddCategoryDto AddCategoryDto { get; set; }
    }
}