using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WalletTransfer.Infrastructure.Persistence
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

            // Aquí registres tus implementaciones de repositorio:
            // services.AddScoped<IWalletRepository, WalletRepository>();
            // services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();
            return services;
        }
    }
}
