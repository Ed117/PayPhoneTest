using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletTransfer.Domain.Interfaces;
using WalletTransfer.Infrastructure.Persistence.Repositories;
using WalletTransfer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WalletTransfer.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<WalletTransferDbContext>(opts =>
                opts.UseMySql(conn, ServerVersion.AutoDetect(conn)));

            // Repositorios
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();

            return services;
        }
    }
}
