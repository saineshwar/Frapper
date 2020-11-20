using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Frapper.ViewModel.Customers;
using Frapper.Web.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frapper.Web.Extensions
{
    public static class FluentValidationCollectionExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator<CustomersViewModel>, CustomerValidator>();
            return services;
        }
    }
}
