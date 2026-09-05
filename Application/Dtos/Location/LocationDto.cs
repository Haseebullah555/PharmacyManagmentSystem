namespace Application.Dtos.Location
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string? Code { get; set; }
        public int? ParentLocationID { get; set; }
        public bool IsActive { get; set; }
    }
}
