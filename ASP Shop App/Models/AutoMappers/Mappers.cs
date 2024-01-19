using ASP_Shop_App.Helpers;
using ASP_Shop_App.Models.ViewModels;
using AutoMapper;

namespace ASP_Shop_App.Models.AutoMappers
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<AddAPViewModel, Product>().ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => UploadFileHelper.UploadFile(src.ImageUrl).Result));
            //CreateMap<ProductUpdateViewModel, Product>();
            CreateMap<AddCategoryViewModel, Category>().ForMember(dest => dest.ImageUrlCategory, opt => opt.MapFrom(src => UploadFileHelper.UploadFile(src.ImageUrlCategory).Result));
        }
    }
}
