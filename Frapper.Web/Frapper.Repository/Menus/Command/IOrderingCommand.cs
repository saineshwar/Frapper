using System.Collections.Generic;
using Frapper.ViewModel.Ordering;

namespace Frapper.Repository.Menus.Command
{
    public interface IOrderingCommand
    {
        void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist);
        void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder);
        void UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder);
    }
}