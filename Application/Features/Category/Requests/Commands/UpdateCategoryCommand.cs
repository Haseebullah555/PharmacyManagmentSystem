using Application.Dtos.Category;
using MediatR;

namespace Application.Features.Category.Requests.Commands
{
    public class UpdateCategoryCommand : IRequest
    {
        public UpdateCategoryDto UpdateCategoryDto { get; set; }
    }
}