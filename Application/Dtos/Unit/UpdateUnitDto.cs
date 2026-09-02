using Application.Dtos.Common;

namespace Application.Dtos.Unit
{
    public class UpdateUnitDto : UpdateBaseDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
    }
}
