using Application.Dtos.UserManagement.Roles;
using Application.Dtos.UserManagement.User;
using AutoMapper;
using Domain.Models.UserManagement;
using Application.Dtos.Company;
using Application.Dtos.Medicine;
using Application.Dtos.Purchase;
using Application.Dtos.Sale;
using Domain.Models;
using Application.Dtos.Category;

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

            CreateMap<Company, AddCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
            CreateMap<Medicine, AddMedicineDto>().ReverseMap();
            CreateMap<Medicine, UpdateMedicineDto>().ReverseMap();
            CreateMap<Purchase, AddPurchaseDto>().ReverseMap();
            CreateMap<Purchase, UpdatePurchaseDto>().ReverseMap();
            CreateMap<Sale, AddSaleDto>().ReverseMap();
            CreateMap<Sale, UpdateSaleDto>().ReverseMap();

            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            CreateMap<Company, AddCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
        }
    }
}
