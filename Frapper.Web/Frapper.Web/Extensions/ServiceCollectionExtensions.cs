using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Repository;
using Frapper.Repository.Audit.Command;
using Frapper.Repository.CacheService;
using Frapper.Repository.Customers.Queries;
using Frapper.Repository.Documents.Queries;
using Frapper.Repository.EmailVerification.Queries;
using Frapper.Repository.Menus.Queries;
using Frapper.Repository.Notices.Queries;
using Frapper.Repository.ProfileImage.Queries;
using Frapper.Repository.Reports.Queries;
using Frapper.Repository.Rolemasters.Command;
using Frapper.Repository.Rolemasters.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.Services.ExcelService;
using Frapper.Services.Messages;
using Frapper.Services.PdfService;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frapper.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Unit of work Using Entity Framework Core
            services.AddTransient<IUnitOfWorkEntityFramework, UnitOfWorkEntityFramework>();
            services.AddTransient<IUnitOfWorkDapper, UnitOfWorkDapper>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            services.AddScoped<IAuditCommand, AuditCommand>();
            services.AddScoped<IRoleCommand, RoleCommand>();
            services.AddScoped<IRoleQueries, RoleQueries>();
            services.AddScoped<IUserMasterQueries, UserMasterQueries>();
            services.AddScoped<IAssignedRolesQueries, AssignedRolesQueries>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserTokensQueries, UserTokensQueries>();
            services.AddScoped<IMenuCategoryQueries, MenuCategoryQueries>();
            services.AddScoped<IMenuMasterQueries, MenuMasterQueries>();
            services.AddScoped<ISubMenuMasterQueries, SubMenuMasterQueries>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IVerificationQueries, VerificationQueries>();
            services.AddScoped<IProfileImageQueries, ProfileImageQueries>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddScoped<IExcelGenerator, ExcelGenerator>();
            services.AddScoped<IExportReportQueries, ExportReportQueries>();
            services.AddScoped<ICustomersQueries, CustomersQueries>();
            services.AddScoped<INoticeQueries, NoticeQueries>();
            services.AddScoped<IDocumentQueries, DocumentQueries>();
            
            services.AddScoped<AuditFilterAttribute>();


            return services;
        }
    }
}
