using AutoMapper;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;

namespace PharmacyManagmentSystem

{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Medicine, MedicineViewModel> ()
                .ReverseMap();
            CreateMap<Purchase, PurchasesViewModel> ()
                .ReverseMap();
        }
    }
}
