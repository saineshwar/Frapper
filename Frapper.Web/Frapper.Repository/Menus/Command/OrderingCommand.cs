using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Frapper.ViewModel.Ordering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace Frapper.Repository.Menus.Command
{
    public class OrderingCommand : IOrderingCommand
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        public OrderingCommand(IDbTransaction dbTransaction, IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _dbTransaction = dbTransaction;
        }

        public void UpdateMenuCategoryOrder(List<MenuCategoryStoringOrder> menuCategorylist)
        {
            foreach (var menuCategory in menuCategorylist)
            {
                var param = new DynamicParameters();
                param.Add("@MenuCategoryId", menuCategory.MenuCategoryId);
                param.Add("@RoleId", menuCategory.RoleId);
                param.Add("@SortingOrder", menuCategory.SortingOrder);
                _dbConnection.Execute("Usp_UpdateMenuCategoryOrder", param, _dbTransaction, 0, CommandType.StoredProcedure);
            }
        }

        public void UpdateMenuOrder(List<MenuStoringOrder> menuStoringOrder)
        {
            foreach (var menu in menuStoringOrder)
            {
                var param = new DynamicParameters();
                param.Add("@MenuId", menu.MenuId);
                param.Add("@RoleId", menu.RoleId);
                param.Add("@SortingOrder", menu.SortingOrder);
                _dbConnection.Execute("Usp_UpdateMenuOrder", param, _dbTransaction, 0, CommandType.StoredProcedure);
            }
        }

        public void UpdateSubMenuOrder(List<SubMenuStoringOrder> submenuStoringOrder)
        {
            foreach (var submenu in submenuStoringOrder)
            {
                var param = new DynamicParameters();
                param.Add("@MenuId", submenu.MenuId);
                param.Add("@RoleId", submenu.RoleId);
                param.Add("@SortingOrder", submenu.SortingOrder);
                param.Add("@SubMenuId", submenu.SubMenuId);
                _dbConnection.Execute("Usp_UpdateSubMenuOrder", param, _dbTransaction, 0, CommandType.StoredProcedure);
            }
        }
    }
}