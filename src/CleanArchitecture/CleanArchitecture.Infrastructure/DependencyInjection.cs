using Asp.Versioning;
using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Application.Paginations;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure.Clock;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Email;
using CleanArchitecture.Infrastructure.Outbox;
using CleanArchitecture.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {

            services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
            services.AddQuartz();
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.ConfigureOptions<ProcessOutboxMessageSetup>();

            services.AddApiVersioning(options =>{
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                
            }).AddMvc()
            .AddApiExplorer( op =>{
                op.GroupNameFormat = "'v'V";
                op.SubstituteApiVersionInUrl = true;
            });
            
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();


            var connectionString = configuration.GetConnectionString("ConnectionString")
            ?? throw new ArgumentNullException("Database connection string is missing in appsettings.json");

            services.AddDbContext<ApplicationDbContext>(options =>{
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaginationUserRepository, UserRepository>();

            services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            services.AddScoped<IPaginationVehiculoRepository, VehiculoRepository>();
            
            services.AddScoped<IAlquilerRepository, AlquilerRepository>();

            services.AddScoped<IUnitOfWork>( sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }
}