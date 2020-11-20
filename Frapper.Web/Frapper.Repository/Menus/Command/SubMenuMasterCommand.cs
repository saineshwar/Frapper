using Frapper.Entities.Menus;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Menus.Command
{
    public class SubMenuMasterCommand : ISubMenuMasterCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public SubMenuMasterCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public void Add(SubMenuMaster subMenuMaster)
        {
            _frapperDbContext.SubMenuMasters.Add(subMenuMaster);
        }

        public void Delete(SubMenuMaster subMenuMaster)
        {
            _frapperDbContext.Entry(subMenuMaster).State = EntityState.Deleted;
        }

        public void Update(SubMenuMaster subMenuMaster)
        {
            _frapperDbContext.Entry(subMenuMaster).State = EntityState.Modified;
        }
    }
}