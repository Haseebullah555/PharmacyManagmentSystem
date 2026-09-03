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
using Application.Dtos.Supplier;
using Application.Dtos.Dosage;
using Application.Dtos.Currency;
using Application.Dtos.Customer;
using Application.Dtos.Inventory;
using Application.Dtos.Unit;
using Application.Dtos.MedicineUnit;
using Application.Dtos.Location;
using Application.Dtos.PurchaseItem;
using Application.Dtos.InventoryTransaction;
using Application.Dtos.InventoryStock;
using Application.Dtos.InventoryBatch;

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

            CreateMap<Dosage, AddDosageDto>().ReverseMap();
            CreateMap<Dosage, UpdateDosageDto>().ReverseMap();

            CreateMap<Supplier, AddSupplierDto>().ReverseMap();
            CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();

            CreateMap<Currency, AddCurrencyDto>().ReverseMap();
            CreateMap<Currency, UpdateCurrencyDto>().ReverseMap();

            CreateMap<Customer, AddCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

            CreateMap<InventoryBatch, AddInventoryBatchDto>().ReverseMap();
            CreateMap<InventoryBatch, UpdateInventoryBatchDto>().ReverseMap();

            CreateMap<InventoryStock, AddInventoryStockDto>().ReverseMap();
            CreateMap<InventoryStock, UpdateInventoryStockDto>().ReverseMap();

            CreateMap<InventoryTransaction, AddInventoryTransactionDto>().ReverseMap();
            CreateMap<InventoryTransaction, UpdateInventoryTransactionDto>().ReverseMap();

            CreateMap<Location, AddLocationDto>().ReverseMap();
            CreateMap<Location, UpdateLocationDto>().ReverseMap();

            CreateMap<MedicineUnit, AddMedicineUnitDto>().ReverseMap();
            CreateMap<MedicineUnit, UpdateMedicineUnitDto>().ReverseMap();

            CreateMap<Purchase, AddPurchaseDto>().ReverseMap();
            CreateMap<Purchase, UpdatePurchaseDto>().ReverseMap();

            CreateMap<PurchaseItem, AddPurchaseItemDto>().ReverseMap();
            CreateMap<PurchaseItem, UpdatePurchaseItemDto>().ReverseMap();

            CreateMap<SaleBatchAllocation, AddSaleBatchAllocationDto>().ReverseMap();
            CreateMap<SaleBatchAllocation, UpdateSaleBatchAllocationDto>().ReverseMap();

            CreateMap<Unit, AddUnitDto>().ReverseMap();
            CreateMap<Unit, UpdateUnitDto>().ReverseMap();

            CreateMap<UserRole, AddUserRoleDto>().ReverseMap();
            CreateMap<UserRole, UpdateUserRoleDto>().ReverseMap();

            CreateMap<RolePermission, AddRolePermissionDto>().ReverseMap();
            CreateMap<RolePermission, UpdateRolePermissionDto>().ReverseMap();

        }
    }
}
