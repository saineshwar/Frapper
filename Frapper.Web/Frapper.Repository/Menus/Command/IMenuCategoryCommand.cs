using Frapper.Entities.Menus;

namespace Frapper.Repository.Menus.Command
{
    public interface IMenuCategoryCommand
    {
        void Add(MenuCategory category);
        void Update(MenuCategory category);
        void Delete(MenuCategory category);
    }
}