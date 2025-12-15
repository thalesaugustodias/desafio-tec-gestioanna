using ConsultaDeCreditos.Application.Services;
using ConsultaDeCreditos.Domain.Interfaces.Mensageria;
using ConsultaDeCreditos.Domain.Interfaces.Repositorios;
using ConsultaDeCreditos.Infrastructure.Mensageria;
using ConsultaDeCreditos.Infrastructure.Persistencia;
using ConsultaDeCreditos.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsultaDeCreditos.IoC;

/// <summary>
/// Configuração de injeção de dependências
/// Centraliza o registro de todos os serviços da aplicação
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AdicionarInfrastrutura(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada. Configure no appsettings.json ou na variável de ambiente 'ConnectionStrings__DefaultConnection'.");
        
        services.AddDbContext<ConsultaCreditosDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(60);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            })
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        });

        services.AddScoped<ICreditoRepositorio, CreditoRepositorio>();

        // Mensageria - Singleton para manter estado da fila em memória
        services.AddSingleton<ServiceBusMemoryProvider>();
        services.AddSingleton<IServiceBusPublisher>(sp => sp.GetRequiredService<ServiceBusMemoryProvider>());
        services.AddSingleton<IServiceBusConsumer>(sp => sp.GetRequiredService<ServiceBusMemoryProvider>());

        return services;
    }

    public static IServiceCollection AdicionarApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.Commands.IntegrarCreditosConstituidosCommand).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddHostedService<ProcessadorMensagensBackgroundService>();

        return services;
    }
}
