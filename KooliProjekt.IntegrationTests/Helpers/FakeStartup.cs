using FluentValidation;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.WebAPI.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers;

// Mirrors the teacher's FakeStartup. Uses real controllers, validators,
// MediatR behaviors, repositories and SQL Server; no mocks and no production seed.
public sealed class FakeStartup
{
    private readonly IConfiguration configuration;
    public FakeStartup(IConfiguration configuration) { this.configuration = configuration; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("TestConnection")));
        services.AddScoped<IOluRepository, OluRepository>();
        services.AddScoped<IPartiiRepository, PartiiRepository>();
        services.AddScoped<IKoostisosaRepository, KoostisosaRepository>();
        services.AddScoped<IMaitsmineRepository, MaitsmineRepository>();
        services.AddScoped<IPartiiFotoRepository, PartiiFotoRepository>();
        services.AddScoped<IPruulimisLogiRepository, PruulimisLogiRepository>();
        var assembly = typeof(ErrorHandlingBehavior<,>).Assembly;
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(assembly);
            c.AddOpenBehavior(typeof(ErrorHandlingBehavior<,>));
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            c.AddOpenBehavior(typeof(TransactionalBehavior<,>));
        });
        services.AddControllers().AddApplicationPart(typeof(ApiControllerBase).Assembly);
    }
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}

