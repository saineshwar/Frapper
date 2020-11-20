namespace Frapper.ViewModel.Ordering
{
    public class MenuCategoryOrderingVm
    {
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
    }

    public class RequestMenuCategoryOrderVM
    {
        public int[] SelectedOrder { get; set; }
        public int RoleId { get; set; }
    }

    public class MenuCategoryStoringOrder
    {
        public int MenuCategoryId { get; set; }
        public int RoleId { get; set; }
        public int SortingOrder { get; set; }
    }
}