using Application.Dtos.Common;

namespace Application.Dtos.Customer
{
    public class UpdateCustomerDto : UpdateBaseDto
    {
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
    }
}
