using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.User
{
    public class AddUserDto : CreateBaseDto
    {
        public string? UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; }
        public List<Guid> roles { get; set; } = new();
    }
}