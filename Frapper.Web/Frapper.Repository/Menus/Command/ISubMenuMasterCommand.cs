using Frapper.Entities.Menus;

namespace Frapper.Repository.Menus.Command
{
    public interface ISubMenuMasterCommand
    {
        void Add(SubMenuMaster subMenuMaster);
        void Delete(SubMenuMaster subMenuMaster);
        void Update(SubMenuMaster subMenuMaster);
    }
}