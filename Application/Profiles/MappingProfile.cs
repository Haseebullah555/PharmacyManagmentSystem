using Application.Dtos.UserManagement.Roles;
using Application.Dtos.UserManagement.User;
using AutoMapper;
using Domain.Models.UserManagement;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User Management
            CreateMap<User, AddUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<Role, AddRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
            #endregion
        }
    }
}