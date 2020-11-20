namespace Frapper.ViewModel.Ordering
{
    public class RequestSubMenuMasterOrderVM
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }

    public class SubMenuStoringOrder
    {
        public int SubMenuId { get; set; }
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }

    public class RequestSubMenu
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }

    public class SubMenuMasterOrderingVm
    {
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
    }


}