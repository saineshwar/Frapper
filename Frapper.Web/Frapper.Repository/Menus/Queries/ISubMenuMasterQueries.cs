using System.Collections.Generic;
using System.Linq;
using Frapper.Entities.Menus;
using Frapper.ViewModel.Menus;
using Frapper.ViewModel.Ordering;

namespace Frapper.Repository.Menus.Queries
{
    public interface ISubMenuMasterQueries
    {
        bool CheckSubMenuNameExists(string subMenuName, int menuId);
        EditSubMenuMasterViewModel GetSubMenuById(int? subMenuId);
        bool CheckSubMenuNameExists(string subMenuName, int? menuId, int? roleId, int? categoryId);
        IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search);
        bool EditValidationCheck(int? subMenuId, EditSubMenuMasterViewModel editsubMenu);
        SubMenuMaster GetSubMenuBySubMenuId(int? subMenuId);
        List<SubMenuMaster> GetSubMenuByRoleId(int? roleId, int? menuCategoryId, int menuid);
        List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(int roleId, int menuid);
    }
}