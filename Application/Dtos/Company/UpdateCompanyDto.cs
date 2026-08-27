using Application.Dtos.Common;

namespace Application.Dtos.Company
{
    public class UpdateCompanyDto : UpdateBaseDto
    {
        public string CompanyName { get; set; }
    }
}
