using System.Collections.Generic;
using Frapper.ViewModel.Customers;

namespace Frapper.Repository.Customers.Queries
{
    public interface ICustomersQueries
    {
        int GetCustomersCount(string search);
        List<CustomersViewModel> CustomerList(string search, string orderBy, int? pageNumber, int pageSize);
    }
}