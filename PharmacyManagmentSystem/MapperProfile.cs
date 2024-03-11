using AutoMapper;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;

namespace PharmacyManagmentSystem

{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MedicineViewModel, Medicine>();
        }
    }
}
