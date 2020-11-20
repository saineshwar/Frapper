namespace Frapper.ViewModel.Ordering
{
    public class RequestMenuMasterOrderVM
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
    }
    public class MenuMasterOrderingVm
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
    public class RequestMenu
    {
        public int RoleId { get; set; }
        public int MenuCategoryId { get; set; }
    }
    public class MenuStoringOrder
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }
}