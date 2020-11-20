
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using DNTCaptcha.Core;
using FluentValidation.AspNetCore;
using Frapper.Common;
using Frapper.Repository;
using Frapper.Web.Extensions;
using Frapper.Web.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Reflection;
using Frapper.Web.Filters;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;


namespace Frapper.Web
{
    extern alias signed;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
            services.AddFluentValidation(Configuration);
            services.AddSingleton<LocalizationService>();
            services.AddAutoMapper(typeof(Startup).Assembly);

            var connection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<FrapperDbContext>(options => options.UseSqlServer(connection));
            services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var redisAppsettingValues = Configuration.GetSection("RedisServer").Get<RedisAppsetting>();

            #region AddDistributedRedisCache
            //services.AddDistributedRedisCache(setup =>
            //{
            //    setup.ConfigurationOptions = new signed::StackExchange.Redis.ConfigurationOptions();
            //    setup.ConfigurationOptions.EndPoints.Add(redisAppsettingValues.Host, Convert.ToInt32(redisAppsettingValues.Port));
            //    setup.ConfigurationOptions.Password = redisAppsettingValues.Password;
            //    setup.ConfigurationOptions.ClientName = "SafeRedisConnection";
            //    setup.ConfigurationOptions.ConnectTimeout = 100000;
            //    setup.ConfigurationOptions.KeepAlive = 30;
            //    setup.ConfigurationOptions.SyncTimeout = 300000;
            //    setup.ConfigurationOptions.ConnectRetry = 5;
            //    setup.ConfigurationOptions.AbortOnConnectFail = false;
            //    setup.ConfigurationOptions.AllowAdmin = false;
            //    setup.ConfigurationOptions.Ssl = true;
            //}); 
            #endregion


            #region Registering HealthChecks
            //services
            //        .AddHealthChecks()
            //        .AddSqlServer(connectionString: Configuration["ConnectionStrings:DatabaseConnection"],
            //            healthQuery: "SELECT 1;",
            //            name: "Sql Server",
            //            failureStatus: HealthStatus.Degraded,
            //            tags: new string[] { "db", "sql", "sqlserver" })
            //        .AddRedis(Configuration["RedisServer:Port"]);

            //services.AddHealthChecksUI(setupSettings: setup =>
            //    {
            //        setup.SetEvaluationTimeInSeconds(5); //Configures the UI to poll for healthchecks updates every 5 seconds
            //    })
            //    .AddSqlServerStorage(Configuration["ConnectionStrings:DatabaseConnection"]); 
            #endregion


            #region Registering DinkToPdf 
            // Registering DinkToPdf 
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            #endregion

            #region Registering ResourcesPath
            services.AddLocalization(options => options.ResourcesPath = "Resources"); 
            #endregion

            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                })
                .AddSessionStateTempDataProvider()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                });



            // For FileUpload
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                x.ValueLengthLimit = int.MaxValue; //not recommended value
                x.MemoryBufferThreshold = Int32.MaxValue;
            });

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            //  Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.OnAppendCookie = (context) =>
                {
                    context.IssueCookie = true;
                };
            });


            #region Localization
            services.Configure<RequestLocalizationOptions>(options =>
              {
                  var supportedCultures = new[]
                  {
                    new CultureInfo("en"),
                    new CultureInfo("mr"),
                    new CultureInfo("hi"),
                    new CultureInfo("ja"),
                    new CultureInfo("de"),
                  };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture("en");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
              }); 
            #endregion

            services.AddControllersWithViews(
                    config =>
                    {
                        config.Filters.Add(typeof(AuditFilterAttribute));
                    }
                )
                //.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            }).AddRazorRuntimeCompilation();

            services.AddControllers();

            // using Memory Cache 
            services.AddMemoryCache();

            #region Registering AddDNTCaptcha
            //  AddDNTCaptcha
            services.AddDNTCaptcha(options =>
                    // options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                    // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                    options.UseCookieStorageProvider() // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                       // options.UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.

                // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
                // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
                // .UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "name.ttf")) 
                // .AbsoluteExpiration(minutes: 7)
                .ShowThousandsSeparators(false)

            ); 
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            // Logging Error 
            //app.UseExceptional();

            // Redirecting to Error Page with Status Code
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();

            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404)
            //    {
            //        context.Request.Path = "/Error/404";
            //        await next();
            //    }

            //    if (context.Response.StatusCode == 500)
            //    {
            //        context.Request.Path = "/Error/500";
            //        await next();
            //    }

            //});

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Localization
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRouting();
            app.UseAuthorization();

            // Enabling Session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Portal}/{action=Login}/{id?}");

                #region HealthChecks
                //// HealthChecks
                //endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
                //{
                //    Predicate = _ => true,
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});

                //// HealthChecks
                //endpoints.MapHealthChecksUI(); 
                #endregion
            });
        }
    }
}
