using Microsoft.Extensions.DependencyInjection;
using WalletTransfer.Application.Interfaces;
using WalletTransfer.Application.Services;

namespace WalletTransfer.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
            return services;
        }
    }
}
