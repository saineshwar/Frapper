using Frapper.ViewModel.Customers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Frapper.Repository.Customers.Queries
{
    public class CustomersQueries : ICustomersQueries
    {

        private readonly IConfiguration _configuration;
        public CustomersQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<CustomersViewModel> CustomerList(string search, string orderBy, int? pageNumber, int pageSize)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection")))
            {
                var para = new DynamicParameters();
                para.Add("@orderBy", orderBy);
                para.Add("@PageNumber", pageNumber);
                para.Add("@PageSize", pageSize);
                para.Add("@Search", search);
                var data = con.Query<CustomersViewModel>("Usp_CustomerPagination", para, commandType: CommandType.StoredProcedure).ToList();
                return data;
            }
        }

        public int GetCustomersCount(string search)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection")))
                {
                    var para = new DynamicParameters();
                    para.Add("@Search", search);
                    var data = con.Query<int>("Usp_GetCustomersCount", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return data;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}