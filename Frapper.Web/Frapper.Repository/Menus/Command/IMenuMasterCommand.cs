using Frapper.Entities.Menus;

namespace Frapper.Repository.Menus.Command
{
    public interface IMenuMasterCommand
    {
        void Add(MenuMaster menuMaster);
        void Delete(MenuMaster menuMaster);
        void Update(MenuMaster menuMaster);
    }
}