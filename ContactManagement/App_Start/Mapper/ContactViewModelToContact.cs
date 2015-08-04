
using AutoMapper;
using ContactManagement.Models;
using ContactManagement.ViewModel;

namespace ContactManagement.Mapper
{
    public class ContactViewModelToContact : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            AutoMapper.Mapper.CreateMap<ContactAddEditViewModel, Contact>();
        }
    }
}