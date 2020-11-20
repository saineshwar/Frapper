using X.PagedList;

namespace Frapper.ViewModel.Customers
{
    public class CustomerPagingInfo
    {
        public int? pageSize;
        public int sortBy;
        public string Search;
        public bool isAsc { get; set; }
        public StaticPagedList<CustomersViewModel> CustomersPagedList { get; set; }
    }
}