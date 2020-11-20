using Frapper.ViewModel.Customers;

namespace Frapper.Repository.Customers.Command
{
    public interface ICustomersCommand
    {
        void Add(CustomersViewModel customersViewModel, long userId);
    }
}