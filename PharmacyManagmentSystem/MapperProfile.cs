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
            CreateMap<PurchasesViewModel, Purchase> ();
            CreateMap<Purchase, PurchasesViewModel> ()
                .ReverseMap();
            CreateMap<Sale, SaleViewModel>()
                .ReverseMap();
        }
    }
}
