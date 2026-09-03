using Application.Dtos.Common;

namespace Application.Dtos.Location
{
    public class AddLocationDto : CreateBaseDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public int? ParentLocationID { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
