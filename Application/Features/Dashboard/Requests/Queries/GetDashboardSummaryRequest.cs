using Application.Dtos.Dashboard;
using MediatR;

namespace Application.Features.Dashboard.Requests.Queries
{
    public class GetDashboardSummaryRequest : IRequest<DashboardSummaryDto>
    {
    }
}
