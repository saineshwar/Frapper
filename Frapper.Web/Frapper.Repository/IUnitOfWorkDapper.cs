using Frapper.Repository.Audit.Command;
using Frapper.Repository.Customers.Command;
using Frapper.Repository.Menus.Command;

namespace Frapper.Repository
{
    public interface IUnitOfWorkDapper
    {
        ICustomersCommand CustomersCommand { get; }
        IOrderingCommand OrderingCommand { get; }
        bool Commit();
    }
}