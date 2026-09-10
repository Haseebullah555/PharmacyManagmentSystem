namespace Application.Dtos.Location
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? ParentLocationId { get; set; }
        public bool IsActive { get; set; }
    }
}
