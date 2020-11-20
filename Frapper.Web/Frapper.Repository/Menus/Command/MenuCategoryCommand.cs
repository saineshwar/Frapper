using Frapper.Entities.Menus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frapper.Repository.Menus.Command
{
    public class MenuCategoryCommand : IMenuCategoryCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public MenuCategoryCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public void Add(MenuCategory category)
        {
            _frapperDbContext.MenuCategorys.Add(category);
        }

        public void Delete(MenuCategory category)
        {
            _frapperDbContext.Entry(category).State = EntityState.Deleted;
        }

        public void Update(MenuCategory category)
        {
            _frapperDbContext.Entry(category).State = EntityState.Modified;
        }

    }
}
