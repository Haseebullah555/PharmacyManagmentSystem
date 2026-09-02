using Application.Dtos.Common;

namespace Application.Dtos.Supplier
{
    public class UpdateSupplierDto : UpdateBaseDto
    {

        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}
