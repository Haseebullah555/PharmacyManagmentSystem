using Application.Dtos.Common;

namespace Application.Dtos.Company
{
    public class AddCompanyDto : CreateBaseDto
    {
        public string CompanyName { get; set; }
    }
}
