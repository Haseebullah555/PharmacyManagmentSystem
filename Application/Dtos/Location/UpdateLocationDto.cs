using Application.Dtos.Common;

namespace Application.Dtos.Location
{
    public class UpdateLocationDto : UpdateBaseDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public int? ParentLocationID { get; set; }
        public bool IsActive { get; set; }
    }
}
