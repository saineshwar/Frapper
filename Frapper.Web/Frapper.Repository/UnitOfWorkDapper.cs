using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Frapper.Repository.Audit.Command;
using Frapper.Repository.Customers.Command;
using Frapper.Repository.Menus.Command;
using Microsoft.Extensions.Configuration;

namespace Frapper.Repository
{
    public class UnitOfWorkDapper : IUnitOfWorkDapper
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private ICustomersCommand _customersCommand;
        private IOrderingCommand _orderingCommand;

        private bool _disposed;
        public UnitOfWorkDapper(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DatabaseConnection"));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IOrderingCommand OrderingCommand => _orderingCommand ??= new OrderingCommand(_transaction, _connection);

        public ICustomersCommand CustomersCommand => _customersCommand ??= new CustomersCommand(_transaction, _connection);

        public bool Commit()
        {
            bool returnValue = true;
            try
            {
                _transaction.Commit();
            }
            catch
            {
                returnValue = false;
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
                ResetRepositories();
            }
            return returnValue;
        }

        private void ResetRepositories()
        {
            _orderingCommand = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWorkDapper()
        {
            Dispose(false);
        }
    }
}
