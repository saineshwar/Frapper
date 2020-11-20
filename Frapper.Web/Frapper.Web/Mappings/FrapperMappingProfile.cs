using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Frapper.Entities.Customers;
using Frapper.Entities.Menus;
using Frapper.Entities.Notices;
using Frapper.Entities.Usermaster;
using Frapper.ViewModel.Customers;
using Frapper.ViewModel.Menus;
using Frapper.ViewModel.Notices;
using Frapper.ViewModel.Registration;
using Frapper.ViewModel.Usermaster;

namespace Frapper.Web.Mappings
{
    public class FrapperMappingProfile : Profile
    {
        public FrapperMappingProfile()
        {
            CreateMap<CreateUserViewModel, UserMaster>();
            CreateMap<RegistrationViewModel, UserMaster>();
            CreateMap<UserMaster, EditUserViewModel>();
            CreateMap<CreateMenuCategoryViewModel, MenuCategory>();
            CreateMap<CreateMenuMasterViewModel, MenuMaster>();
            CreateMap<CreateSubMenuMasterViewModel, SubMenuMaster>();
            CreateMap<CustomersViewModel, Customers>();
            CreateMap<CreateNoticeViewModel, Notice>();
            CreateMap<EditNoticeViewModel, Notice>();
        }
    }
}
