using System.Collections.Generic;
using System.Linq;
using Frapper.Entities.Rolemasters;
using Frapper.ViewModel.Rolemasters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.Repository.Rolemasters.Queries
{
    public interface IRoleQueries
    {
        bool CheckRoleNameExists(string roleName);
        IQueryable<RoleMasterGrid> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search);
        RoleMaster GetRoleMasterByroleId(int? roleId);
        EditRoleMasterViewModel GetRoleMasterForEditByroleId(int? roleId);
        List<SelectListItem> ListofRoles();
    }
}