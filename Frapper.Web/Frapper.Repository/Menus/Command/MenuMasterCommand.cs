using Frapper.Entities.Menus;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Menus.Command
{
    public class MenuMasterCommand : IMenuMasterCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public MenuMasterCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Add(MenuMaster category)
        {
            _frapperDbContext.MenuMasters.Add(category);
        }

        public void Delete(MenuMaster category)
        {
            _frapperDbContext.Entry(category).State = EntityState.Deleted;
        }

        public void Update(MenuMaster category)
        {
            _frapperDbContext.Entry(category).State = EntityState.Modified;
        }
    }
}